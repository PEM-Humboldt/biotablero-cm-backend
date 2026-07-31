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

        Indicator indicator = null;

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
        }

        // Validate data
        foreach (var row in fileReadResult.Rows)
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

        // Validate general data
        var indicatorsWithoutGroupRequired = new IndicatorTypes[]
        {
            IndicatorTypes.SpeciesDiversity,
        };

        var indicatorsWithSpecies = new IndicatorTypes[]
        {
            IndicatorTypes.OccupiedAreaPercent,
            IndicatorTypes.DetectionOccupancyProbability,
            IndicatorTypes.RelativeUseByBiologicalGroup,
        };

        var indicatorsWithConfidenceInterval = new IndicatorTypes[]
        {
            IndicatorTypes.DetectionOccupancyProbability,
            IndicatorTypes.SpeciesDiversity,
        };

        var indicatorsWithFinalDate = new IndicatorTypes[]
        {
            IndicatorTypes.RelativeUseByBiologicalGroup,
            IndicatorTypes.CollectiveActionParticipation,
        };

        foreach (var row in fileReadResult.Rows)
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

            if (indicatorsWithSpecies.Contains((IndicatorTypes)row.IndicatorTypeId))
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

            if (indicatorsWithoutGroupRequired.Contains((IndicatorTypes)row.IndicatorTypeId))
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

            if (indicatorsWithConfidenceInterval.Contains((IndicatorTypes)row.IndicatorTypeId))
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

            if (indicatorsWithFinalDate.Contains((IndicatorTypes)row.IndicatorTypeId))
            {
                if (row.FinalYear == null || row.FinalMonth == null)
                {
                    return new(true)
                    {
                        ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.FinalDateRequired),
                        Message = $"Errors in row {row.RowNumber}",
                    };
                }

                var initDate = DateTime.ParseExact($"{row.Year}-{PrintMonth(row.Month)}-01", GeneralConstants.DateFormat, CultureInfo.InvariantCulture);
                var endDate = DateTime.ParseExact($"{row.FinalYear}-{PrintMonth(row.FinalMonth)}-01", GeneralConstants.DateFormat, CultureInfo.InvariantCulture);

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

        var totalIndicators = fileReadResult.Rows
            .GroupBy(r => r.IndicatorTypeId)
            .Count();

        if (indicator != null && totalIndicators != 1)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.OnlyOneIndicatorRequired),
            };
        }

        /* DATABASE VALIDATIONS */
        // TODO: add this section in a private function

        // Validate upper groups
        var upperGroups = fileReadResult.Rows
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
        var locations = fileReadResult.Rows
            .Select(r => new LocationDataHelper
            {
                Department = r.DepartmentName,
                Municipality = r.MunicipalityName,
            })
            .DistinctBy(r => new { r.Department, r.Municipality })
            .ToArray();

        var locationEntities = await locationRepository.GetByNamesAsync(locations, ct);

        if (locations.Length != locationEntities.Count)
        {
            var locationEntitiesKeyValuePairs = locationEntities
                .Select(e => new LocationDataHelper
                {
                    Department = e.Parent.Name,
                    Municipality = e.Name,
                });

            foreach (var location in locations)
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

        var allCategoryEntities = (await categoryRepository.ListAsync(ct))
            .Select(e => new GroupDataHelper
            {
                Id = e.Id,
                ParentId = e.ParentId,
                Name = e.Name,
                Description = e.Description,
                ParentName = e.Parent?.Name,
            })
            .ToArray();

        var duplicatedCategories = new List<CategoryComparisonHelper>();

        foreach (var category in spreadsheetCategories)
        {
            var duplicatedCategory = allCategoryEntities
                .FirstOrDefault(e =>
                    category.Name.Trim().Equals(e.Name, StringComparison.OrdinalIgnoreCase) &&
                    category.ParentName.Trim().Equals(e.ParentName, StringComparison.OrdinalIgnoreCase));

            if (duplicatedCategory != null)
            {
                duplicatedCategories.Add(new()
                {
                    CategorySpreadsheet = category,
                    CategoryDb = duplicatedCategory,
                });
            }
        }

        result.Warnings.Add("duplicatedCategories", duplicatedCategories);

        if (!requestData.DoNotModifyDatabase)
        {
            var existentCategories = allCategoryEntities
                .Where(i => spreadsheetCategories.Contains(i))
                .ToList();

            var newCategories = allCategoryEntities
                .Where(i => !spreadsheetCategories.Contains(i))
                .ToList();

            // Generate IndicatorVersion entities
            var indicatorVersionEntities = fileReadResult.Rows
                .GroupBy(r => r.IndicatorTypeId)
                .Select(async g => new IndicatorVersion()
                {
                    CreationDate = now,
                    Version = indicator == null ? 1 : await indicatorVersionRepository.GetLastVersion(indicator.Id, ct),
                    Groups = [.. g.GroupBy(g => new { g.UpperGroupName, g.GroupName, g.GroupDescription })
                        .Select(g2 => new IndicatorGroup()
                        {
                            CategoryId =
                                existentCategories
                                    .FirstOrDefault(i => i.ParentName == g2.Key.UpperGroupName && i.Name == g2.Key.GroupName && i.Description == g2.Key.GroupDescription)?.Id ??
                                existentCategories
                                    .FirstOrDefault(i => i.Name == g2.Key.UpperGroupName && i.ParentName == null && g2.Key.GroupName == null && i.Description == null && g2.Key.GroupDescription == null)?.Id ??
                                0,
                            Category = newCategories
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
                                var enabledFinalDate = DateTime.TryParseExact($"{g2r.FinalYear}-{PrintMonth(g2r.FinalMonth)}-01", GeneralConstants.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var finalDate);

                                return new IndicatorValue()
                                {
                                    MeasureUnitId = g2r.MeasureUnitId,
                                    Date = DateTime.ParseExact($"{g2r.Year}-{PrintMonth(g2r.Month)}-01", GeneralConstants.DateFormat, CultureInfo.InvariantCulture),
                                    DateEnd = enabledFinalDate ? finalDate : null,
                                    Value = g2r.Value,
                                    UpperLimit = g2r.UpperLimit,
                                    LowerLimit = g2r.LowerLimit,
                                };
                            })],
                        })],
                })
                .Select(e => e.Result);

            if (!requestData.Id.HasValue)
            {
                var indicators = fileReadResult.Rows
                    .GroupBy(r => r.IndicatorTypeId)
                    .Select(g => new Indicator()
                    {
                        InitiativeId = requestData.InitiativeId,
                        Name = $"{g.Select(r => r.IndicatorTypeId).FirstOrDefault()} ({now.ToFileTime})",
                        Type = new() { Id = g.Key },
                        IndicatorLocations = [.. g.Select(r => new IndicatorLocation()
                        {
                            LocationId = locationEntities.FirstOrDefault(l => l.Name == r.MunicipalityName && l.Parent?.Name == r.DepartmentName)?.Id ?? 0,
                            Locality = r.LocalityName,
                        })],
                        Versions = [.. indicatorVersionEntities],
                    });

                // Save data
                indicators = await entityRepository.AddRangeAsync(indicators, ct);

                var indicatorDtos = indicators
                    .Select(mapper.Map);

                logger.AddLog(LogType.Create, "Added indicators", "{@EntityData}", indicatorDtos);

                result.Result = indicatorDtos;
            }

            // Save data
            indicatorVersionEntities = await indicatorVersionRepository.AddRangeAsync(indicatorVersionEntities, ct);

            var indicatorVersionDtos = indicatorVersionEntities
                .Select(indicatorVersionMapper.Map);

            logger.AddLog(LogType.Create, "Added indicator versions", "{@EntityData}", indicatorVersionDtos);

            result.Result = indicatorVersionDtos;
        }

        return new()
        {
            ResponseBody = result,
        };
    }

    /// <summary>
    /// Print formatted month.
    /// </summary>
    /// <param name="month">Month number as string.</param>
    /// <returns>Month with leading zeros.</returns>
    private static string PrintMonth(string month) => month?.PadLeft(2, '0');
}
