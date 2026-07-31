namespace IAVH.BioTablero.CM.Application.Services.Indicators;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.DTOs.Indicators;
using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices.Spreadsheets.Services;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Indicators;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;
using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;
using IAVH.BioTablero.CM.Core.Domain.Models.Spreadsheets;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;
using IAVH.BioTablero.CM.Core.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Indicators;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Locations;

using Microsoft.AspNetCore.OData.Query;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

using IndicatorMeasureUnits = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.IndicatorsEnums.IndicatorMeasureUnit;
using IndicatorTypes = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.IndicatorsEnums.IndicatorType;

/// <summary>
/// Indicator service.
/// </summary>
public class IndicatorService : ServiceRead<Indicator, IndicatorDto, int>, IIndicatorService
{
    private new readonly IIndicatorRepository entityRepository;
    private readonly ILogger logger;
    private readonly IIndicatorExcelService excelService;
    private readonly ILocationRepository locationRepository;
    private readonly IIndicatorVersionRepository indicatorVersionRepository;
    private readonly ICategoryRepository categoryRepository;
    private readonly IIndicatorLocationRepository indicatorLocationRepository;
    private readonly IValidator<IndicatorsImportRow> indicatorsImportRowValidator;
    private readonly IMapperRead<IndicatorVersion, IndicatorVersionDto> indicatorVersionMapper;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="errorTranslator">Error translator.</param>
    /// <param name="excelService">Excel service.</param>
    /// <param name="locationRepository">Location repository.</param>
    /// <param name="indicatorVersionRepository">Indicator version repository.</param>
    /// <param name="categoryRepository">Indicator Category repository.</param>
    /// <param name="indicatorLocationRepository">Indicator Location repository.</param>
    /// <param name="indicatorsImportRowValidator">Indicators spreadsheet row validator.</param>
    /// <param name="indicatorVersionMapper">Indicator version mapper.</param>
    public IndicatorService(
        IIndicatorRepository entityRepository,
        ILogger logger,
        IMapperRead<Indicator, IndicatorDto> mapper,
        IValidationErrorTranslator errorTranslator,
        IIndicatorExcelService excelService,
        ILocationRepository locationRepository,
        IIndicatorVersionRepository indicatorVersionRepository,
        ICategoryRepository categoryRepository,
        IIndicatorLocationRepository indicatorLocationRepository,
        IValidator<IndicatorsImportRow> indicatorsImportRowValidator,
        IMapperRead<IndicatorVersion, IndicatorVersionDto> indicatorVersionMapper)
    : base(entityRepository, mapper, errorTranslator)
    {
        this.entityRepository = entityRepository;
        this.logger = logger;
        this.excelService = excelService;
        this.locationRepository = locationRepository;
        this.indicatorVersionRepository = indicatorVersionRepository;
        this.categoryRepository = categoryRepository;
        this.indicatorLocationRepository = indicatorLocationRepository;
        this.indicatorsImportRowValidator = indicatorsImportRowValidator;
        this.indicatorVersionMapper = indicatorVersionMapper;
    }

    /// <inheritdoc/>
    public override async Task<CustomWebResponse> GetListAsync(ODataQueryOptions<Indicator> queryOptions, CancellationToken ct = default)
    {
        var query = entityRepository.GetQueryable();
        query = entityRepository.IncludeOdataEntities(query);

        return await GetOdataListByQueryAsync(query, queryOptions, ct);
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetByInitiativeAsync(int initiativeId, CancellationToken ct = default)
    {
        var dataListEntity = await entityRepository.GetByInitiativeAsync(initiativeId, ct);

        var dataListDto = dataListEntity
            .Select(mapper.Map);

        return new()
        {
            ResponseBody = dataListDto,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> ImportIndicatorsAsync(string userName, IndicatorsImportFileDto requestData, IInputFile formFile, CancellationToken ct)
    {
        var result = new SpreadsheetUploadResult()
        {
            DoNotModifyDatabase = requestData.DoNotModifyDatabase,
        };
        var fileReadResult = excelService.GetFileData(formFile);

        if (fileReadResult.Errors.Count > 0)
        {
            return new(true)
            {
                ResponseBody = fileReadResult.Errors,
            };
        }

        // Normalize names
        NormalizeNames(fileReadResult.Rows);

        // Validate indicator
        Indicator indicator = null;
        var indicatorLastVersion = 1;

        if (requestData.Id.HasValue)
        {
            indicator = await entityRepository.GetByIdAsync(requestData.Id.Value, ct);

            if (indicator == null)
            {
                return new(true)
                {
                    ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.NotFound),
                };
            }

            indicatorLastVersion = await indicatorVersionRepository.GetLastVersion(indicator.Id, ct);
        }

        // Make structure validations
        var structureDataValidations = await ValidateStructureData(fileReadResult.Rows, requestData.Id.HasValue, ct);

        if (!structureDataValidations.Success)
        {
            return structureDataValidations;
        }

        // Make database validations
        var spreadsheetLocations = fileReadResult.Rows
            .Select(r => new LocationDataHelper
            {
                Department = r.DepartmentName,
                Municipality = r.MunicipalityName,
                Locality = r.LocalityName,
            })
            .DistinctBy(r => new { r.Department, r.Municipality, r.Locality })
            .ToArray();

        var locationEntities = await locationRepository.GetByNamesAsync(
            [.. spreadsheetLocations.Select(e => e.Department)],
            [.. spreadsheetLocations.Select(e => e.Municipality)],
            ct);

        var databaseValidations = await ValidateDatabase(fileReadResult.Rows, locationEntities, ct);

        if (!databaseValidations.Success)
        {
            return databaseValidations;
        }

        // Save data
        if (!requestData.DoNotModifyDatabase)
        {
            var now = DateTimeOffset.Now;

            // Get groups (categories) data
            var spreadsheetCategories = fileReadResult.Rows
                .Select(r => new GroupDataHelper
                {
                    Name = r.GroupName,
                    Description = r.GroupDescription,
                    ParentName = r.UpperGroupName,
                })
                .DistinctBy(r => new { r.Name, r.Description, r.ParentName })
                .ToArray();

            var databaseCategories = (await categoryRepository.ListAsync(ct))
                .Select(e => new GroupDataHelper
                {
                    Id = e.Id,
                    ParentId = e.ParentId,
                    Name = e.Name.Trim().CapitalizeFirstOnly(),
                    Description = e.Description,
                    ParentName = e.Parent?.Name?.Trim()?.CapitalizeFirstOnly(),
                })
                .ToArray();

            var existentCategories = spreadsheetCategories
                .Join(databaseCategories, sc => new { sc.Name, sc.ParentName }, dbc => new { dbc.Name, dbc.ParentName }, (sc, dbc) => new { sc, dbc })
                .Select(i => i.dbc)
                .Distinct()
                .ToList();

            var newCategories = new List<GroupDataHelper>();

            foreach (var category in spreadsheetCategories)
            {
                var entityExists = databaseCategories.Any(i => category.Name == i.Name && category.ParentName == i.ParentName);
                if (!entityExists && !newCategories.Contains(category))
                {
                    newCategories.Add(category);
                }
            }

            var existentIndicatorLocations = indicator != null ? await indicatorLocationRepository.GetByIndicatorAsync(indicator?.Id ?? 0, ct) : [];

            if (existentIndicatorLocations.Count > 0)
            {
                existentIndicatorLocations = [.. existentIndicatorLocations.Where(e => spreadsheetLocations.Any(i => i.Municipality == e.Location.Name && i.Department == e.Location.Parent.Name && i.Locality == e.Locality))];
            }

            // Generate IndicatorVersion entities
            var indicatorVersionEntities = fileReadResult.Rows
                .GroupBy(r => r.IndicatorTypeId)
                .Select(g => new IndicatorVersion()
                {
                    IndicatorTypeId = g.Key,
                    IndicatorId = indicator?.Id ?? 0,
                    CreationDate = now,
                    Version = indicator == null ? 1 : indicatorLastVersion + 1,
                    Groups = [.. g.GroupBy(g => new { g.UpperGroupName, g.GroupName, g.GroupDescription })
                        .Select(g2 =>
                        {
                            var categoryId = existentCategories
                                    .FirstOrDefault(i => i.ParentName == g2.Key.UpperGroupName && i.Name == g2.Key.GroupName)?.Id ??
                                existentCategories
                                    .FirstOrDefault(i => i.Name == g2.Key.UpperGroupName.Trim() && i.ParentName == null && g2.Key.GroupName == null && i.Description == null && g2.Key.GroupDescription == null)?.Id ??
                                0;

                            return new IndicatorGroup()
                            {
                                CategoryId = categoryId,
                                Category = categoryId != 0 ? null : newCategories
                                    .Where(i => i.ParentName == g2.Key.UpperGroupName && i.Name == g2.Key.GroupName && i.Description == g2.Key.GroupDescription)
                                    .Select(i => new Category()
                                    {
                                        ParentId = i.ParentId,
                                        Name = i.Name,
                                        Description = i.Description,
                                    })
                                    .FirstOrDefault(),
                                Values = [.. g2.Select(g2r =>
                                {
                                    return new IndicatorValue()
                                    {
                                        MeasureUnitId = g2r.MeasureUnitId,
                                        Date = CastDate(g2r.Year, g2r.Month) ?? default,
                                        DateEnd = CastDate(g2r.FinalYear, g2r.FinalMonth),
                                        Value = g2r.Value,
                                        UpperLimit = g2r.UpperLimit,
                                        LowerLimit = g2r.LowerLimit,
                                    };
                                })],
                            };
                        })],
                })
                .ToList();

            if (!requestData.Id.HasValue)
            {
                var indicators = fileReadResult.Rows
                    .GroupBy(r => r.IndicatorTypeId)
                    .Select(g =>
                    {
                        var indicatorsLocations = g
                            .Select(r =>
                            {
                                IndicatorLocation indicatorLocation = null;

                                indicatorLocation = existentIndicatorLocations
                                    .FirstOrDefault(i => i.Location.Name == r.MunicipalityName && i.Location.Parent.Name == r.DepartmentName);

                                if (indicatorLocation == null)
                                {
                                    var locationEntity = locationEntities
                                        .FirstOrDefault(i => i.Name == r.MunicipalityName && i.Parent.Name == r.DepartmentName);

                                    indicatorLocation = new IndicatorLocation()
                                    {
                                        IndicatorId = indicator?.Id ?? 0,
                                        LocationId = locationEntity.Id,
                                        Location = locationEntity,
                                        Locality = r.LocalityName,
                                    };
                                }

                                return indicatorLocation;
                            })
                            .DistinctBy(e => new { e.Id, e.LocationId })
                            .ToList();

                        return new Indicator()
                        {
                            InitiativeId = requestData.InitiativeId,
                            Name = $"Indicador tipo {g.Key} ({now.ToString(GeneralConstants.DatetimeFormat, CultureInfo.CurrentCulture)})",
                            IndicatorTypeId = g.Key,
                            IndicatorLocations = indicatorsLocations,
                            Versions = [.. indicatorVersionEntities.Where(e => e.IndicatorTypeId == g.Key)],
                        };
                    })
                    .ToList();

                // Save data
                await entityRepository.AddRangeAsync(indicators, ct);

                var indicatorDtos = indicators
                    .Select(mapper.Map)
                    .ToList();

                logger.AddLog(LogType.Create, "Added indicators", "{@EntityData}", indicatorDtos);

                result.Result = indicatorDtos;
            }
            else
            {
                // Save data
                await indicatorVersionRepository.AddRangeAsync(indicatorVersionEntities, ct);

                var indicatorVersionDtos = indicatorVersionEntities
                    .Select(indicatorVersionMapper.Map);

                logger.AddLog(LogType.Create, "Added indicator versions", "{@EntityData}", indicatorVersionDtos);

                result.Result = indicatorVersionDtos;
            }
        }

        return new()
        {
            ResponseBody = result,
        };
    }

    #region Import Indicators functions

    /// <summary>
    /// Cast indicator value date.
    /// </summary>
    /// <param name="year">Indicator value year.</param>
    /// <param name="month">Indicator value month.</param>
    /// <returns>DateTime from strings.</returns>
    /// <exception cref="InvalidCastException">Cast date error.</exception>
    private static DateTime? CastDate(string year, string month)
    {
        if (string.IsNullOrEmpty(year) || string.IsNullOrEmpty(month))
        {
            return null;
        }

        var formattedMont = month?.PadLeft(2, '0');
        var parseSuccessful = DateTime.TryParseExact(
            string.Format(
                GeneralConstants.DefaultFormatProvider,
                IndicatorConstants.IndicatorDateFormat,
                year,
                formattedMont),
            GeneralConstants.DateFormat,
            GeneralConstants.DefaultFormatProvider,
            DateTimeStyles.None,
            out var date);

        if (!parseSuccessful)
        {
            throw new InvalidCastException($"Cast date error. year: {year}, month: {month} ");
        }

        return date;
    }

    /// <summary>
    /// Normalize spreadsheet rows names.
    /// </summary>
    /// <param name="rows">Spreadsheet rows.</param>
    private static void NormalizeNames(List<IndicatorsImportRow> rows)
    {
        foreach (var item in rows)
        {
            item.UpperGroupName = item.UpperGroupName.Trim().CapitalizeFirstOnly();
            item.GroupName = item.GroupName.Trim().CapitalizeFirstOnly();
            item.LocalityName = item.LocalityName.Trim().CapitalizeFirstOnly();
        }
    }

    /// <summary>
    /// Spreadsheet structure data validations.
    /// </summary>
    /// <param name="rows">Spreadsheet rows.</param>
    /// <param name="edition">indicator edition flag.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Validation result.</returns>
    private async Task<CustomWebResponse> ValidateStructureData(List<IndicatorsImportRow> rows, bool edition, CancellationToken ct = default)
    {
        foreach (var row in rows)
        {
            var validationResult = await indicatorsImportRowValidator.ValidateAsync(row, ct);

            if (!validationResult.IsValid)
            {
                return new(true)
                {
                    ResponseBody = errorTranslator.Translate(validationResult.Errors),
                    Message = $"Errors in row {row.RowNumber}",
                };
            }
        }

        foreach (var row in rows)
        {
            if (!Enum.GetValues<IndicatorTypes>().Select(e => (int)e).Contains(row.IndicatorTypeId))
            {
                return new(true)
                {
                    ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.InvalidIndicatorType),
                    Message = $"Errors in row {row.RowNumber}",
                };
            }

            foreach (var measureUnit in IndicatorConstants.UnitMeasuresByIndicatorType)
            {
                if (row.IndicatorTypeId == (int)measureUnit.Key && !measureUnit.Value.Contains((IndicatorMeasureUnits)row.MeasureUnitId))
                {
                    return new(true)
                    {
                        ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.InvalidMeasureUnit),
                        Message = $"Errors in row {row.RowNumber}",
                    };
                }
            }

            if (IndicatorConstants.IndicatorsWithSpecies.Contains((IndicatorTypes)row.IndicatorTypeId))
            {
                if (string.IsNullOrEmpty(row.GroupName) || string.IsNullOrEmpty(row.GroupDescription))
                {
                    return new(true)
                    {
                        ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.GroupAndDescriptionRequired),
                        Message = $"Errors in row {row.RowNumber}",
                    };
                }
            }

            if (IndicatorConstants.IndicatorsWithoutGroupRequired.Contains((IndicatorTypes)row.IndicatorTypeId))
            {
                if (!string.IsNullOrEmpty(row.GroupName) || !string.IsNullOrEmpty(row.GroupDescription))
                {
                    return new(true)
                    {
                        ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.GroupAndDescriptionNotRequired, data: $"{row.GroupName}, {row.GroupDescription}"),
                        Message = $"Errors in row {row.RowNumber}",
                    };
                }
            }

            if (IndicatorConstants.IndicatorsWithConfidenceInterval.Contains((IndicatorTypes)row.IndicatorTypeId))
            {
                if (row.UpperLimit == null || row.LowerLimit == null)
                {
                    return new(true)
                    {
                        ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.ConfidenceIntervalRequired),
                        Message = $"Errors in row {row.RowNumber}",
                    };
                }
            }

            if (row.UpperLimit.HasValue && row.LowerLimit.HasValue && (row.Value > row.UpperLimit || row.Value < row.LowerLimit))
            {
                return new(true)
                {
                    ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.InvalidConfidenceInterval),
                    Message = $"Errors in row {row.RowNumber}",
                };
            }

            if (IndicatorConstants.IndicatorsWithDateRange.Contains((IndicatorTypes)row.IndicatorTypeId))
            {
                if (row.FinalYear == null || row.FinalMonth == null)
                {
                    return new(true)
                    {
                        ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.FinalDateRequired),
                        Message = $"Errors in row {row.RowNumber}",
                    };
                }

                var initDate = CastDate(row.Year, row.Month);
                var endDate = CastDate(row.FinalYear, row.FinalMonth);

                if (!(initDate < endDate))
                {
                    return new(true)
                    {
                        ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.InvalidDateRange),
                        Message = $"Errors in row {row.RowNumber}",
                    };
                }
            }
            else
            {
                if (row.FinalYear != null || row.FinalMonth != null)
                {
                    return new(true)
                    {
                        ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.FinalDateNotRequired),
                        Message = $"Errors in row {row.RowNumber}",
                    };
                }
            }
        }

        var totalIndicators = rows
            .GroupBy(r => r.IndicatorTypeId)
            .Count();

        if (edition && totalIndicators != 1)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.OnlyOneIndicatorRequired),
            };
        }

        return new();
    }

    /// <summary>
    /// Spreadsheet database validations.
    /// </summary>
    /// <param name="rows">Spreadsheet rows.</param>
    /// <param name="locationEntities">Location entities list.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Validation result.</returns>
    private async Task<CustomWebResponse> ValidateDatabase(List<IndicatorsImportRow> rows, List<Location> locationEntities, CancellationToken ct = default)
    {
        // Validate upper groups
        var upperGroups = rows
            .Select(r => r.UpperGroupName)
            .Distinct()
            .ToArray();

        var upperGroupEntities = await categoryRepository.GetUpperGroupsAsync(upperGroups, ct);

        if (upperGroups.Length != upperGroupEntities.Count)
        {
            var upperGroupEntitiesStr = upperGroupEntities
                .Select(e => e.Name);

            foreach (var upperGroup in upperGroups)
            {
                if (!upperGroupEntitiesStr.Contains(upperGroup))
                {
                    return new(true)
                    {
                        ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.UpperGroupNotFound, data: $"{upperGroup}"),
                    };
                }
            }
        }

        // Get locations data
        var spreadsheetLocations = rows
            .Select(r => new LocationDataHelper
            {
                Department = r.DepartmentName,
                Municipality = r.MunicipalityName,
            })
            .DistinctBy(r => new { r.Department, r.Municipality })
            .ToArray();

        locationEntities = [.. locationEntities.Where(e => spreadsheetLocations.Any(i => i.Municipality == e.Name && i.Department == e.Parent.Name))];

        if (spreadsheetLocations.Length != locationEntities.Count)
        {
            var locationEntitiesKeyValuePairs = locationEntities
                .Select(e => new LocationDataHelper
                {
                    Department = e.Parent.Name,
                    Municipality = e.Name,
                });

            foreach (var location in spreadsheetLocations)
            {
                if (!locationEntitiesKeyValuePairs.Contains(location))
                {
                    return new(true)
                    {
                        ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.LocationNotFound, data: $"{location}"),
                    };
                }
            }
        }

        return new();
    }

    #endregion
}
