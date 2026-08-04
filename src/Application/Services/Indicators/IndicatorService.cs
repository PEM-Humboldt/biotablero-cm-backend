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
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Locations;

using Microsoft.AspNetCore.OData.Query;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

using IndicatorBaseCategory = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.IndicatorsEnums.IndicatorBaseCategory;
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
    private readonly IInitiativeRepository initiativeRepository;
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
    /// <param name="initiativeRepository">Initiative repository.</param>
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
        IInitiativeRepository initiativeRepository,
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
        this.initiativeRepository = initiativeRepository;
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
    public async Task<CustomWebResponse> ImportIndicatorsAsync(string userName, IndicatorsImportFileDto requestData, IInputFile formFile, CancellationToken ct = default)
    {
        var result = new SpreadsheetUploadResult()
        {
            DoNotModifyDatabase = requestData.DoNotModifyDatabase,
        };
        var fileReadResult = excelService.GetFileData(formFile);

        if (fileReadResult.Errors.Count > 0)
        {
            result.Errors = fileReadResult.Errors;
            return new(true)
            {
                ResponseBody = result,
            };
        }

        // Validate initiative
        if (!await initiativeRepository.AnyAsync(requestData.InitiativeId, ct))
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Initiatives.NotFound),
            };
        }

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

            indicatorLastVersion = await indicatorVersionRepository.GetLastVersionAsync(indicator.Id, ct);
        }

        // Adjust spreadsheet rows data
        AdjustRowsData(fileReadResult.Rows);

        // Make structure validations
        var structureDataValidations = await ValidateStructureDataAsync(fileReadResult.Rows, indicator?.Type, ct);

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

        var databaseValidations = await ValidateDatabaseAsync(fileReadResult.Rows, locationEntities, ct);

        if (!databaseValidations.Success)
        {
            return databaseValidations;
        }

        // Save data
        if (!requestData.DoNotModifyDatabase)
        {
            var now = DateTimeOffset.Now;

            var indicatorLocations = await GetExistingIndicatorLocationsAsync(indicator, spreadsheetLocations, ct);
            var categories = await SaveAndGetCategoriesAsync(fileReadResult.Rows, ct);
            var indicatorVersionEntities = GenerateIndicatorVersions(indicator, fileReadResult.Rows, categories, now, indicatorLastVersion);

            if (!requestData.Id.HasValue)
            {
                var indicators = GenerateIndicators(requestData.InitiativeId, indicator, fileReadResult.Rows, indicatorVersionEntities, indicatorLocations, locationEntities, now);

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

        result.SuccessfulProcess = true;
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
    /// Adjust rows data.
    /// </summary>
    /// <param name="rows">Spreadsheet rows.</param>
    private static void AdjustRowsData(List<IndicatorsImportRow> rows)
    {
        for (int i = 0; i < rows.Count; i++)
        {
            var row = rows[i];

            // Normalize names
            row.UpperGroupName = row.UpperGroupName.Trim().CapitalizeFirstOnly();
            row.GroupName = row.GroupName.Trim().CapitalizeFirstOnly();
            row.LocalityName = row.LocalityName.Trim().CapitalizeFirstOnly();

            // Remove rows with "Total" group
            if (row.GroupName == IndicatorConstants.TotalGroupName)
            {
                rows.RemoveAt(i);
            }

            // Adjust data for "SpeciesDiversity" indicator
            if (row.IndicatorTypeId == (int)IndicatorTypes.SpeciesDiversity)
            {
                row.GroupName = row.UpperGroupName;
                row.UpperGroupName = IndicatorConstants.SpeciesCategoryName;
            }
        }
    }

    /// <summary>
    /// Spreadsheet structure data validations.
    /// </summary>
    /// <param name="rows">Spreadsheet rows.</param>
    /// <param name="type">Indicator type (optional).</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Validation result.</returns>
    private async Task<CustomWebResponse> ValidateStructureDataAsync(List<IndicatorsImportRow> rows, IndicatorType type, CancellationToken ct = default)
    {
        // Validate total indicators for edition
        if (type != null)
        {
            var totalIndicators = rows
            .GroupBy(r => r.IndicatorTypeId)
            .Count();

            if (totalIndicators != 1)
            {
                return new(true)
                {
                    ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.OnlyOneIndicatorRequired),
                };
            }
        }

        foreach (var row in rows)
        {
            // Check FluentValidation validations
            var validationResult = await indicatorsImportRowValidator.ValidateAsync(row, ct);

            if (!validationResult.IsValid)
            {
                return new(true)
                {
                    ResponseBody = errorTranslator.Translate(validationResult.Errors),
                    Message = $"Errors in row {row.RowNumber}",
                };
            }

            // Check indicator types
            if (!Enum.GetValues<IndicatorTypes>().Select(e => (int)e).Contains(row.IndicatorTypeId))
            {
                return new(true)
                {
                    ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.InvalidIndicatorType),
                    Message = $"Errors in row {row.RowNumber}",
                };
            }

            // Check type from original indicator
            if (type != null && row.IndicatorTypeId != type.Id)
            {
                return new(true)
                {
                    ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.InvalidIndicatorType),
                    Message = $"Errors in row {row.RowNumber}. Original type id: {type.Id}, Spreadsheet type id: {row.IndicatorTypeId}",
                };
            }

            // Check indicator measure units
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

            // Check indicators with species
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

            // Check indicators with confidence interval
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

            // Check confidence intervals
            if (row.UpperLimit.HasValue && row.LowerLimit.HasValue && (row.Value > row.UpperLimit || row.Value < row.LowerLimit))
            {
                return new(true)
                {
                    ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.InvalidConfidenceInterval),
                    Message = $"Errors in row {row.RowNumber}",
                };
            }

            // Check indicators with date ranges
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
                // Check indicators without date ranges
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

        return new();
    }

    /// <summary>
    /// Spreadsheet database validations.
    /// </summary>
    /// <param name="rows">Spreadsheet rows.</param>
    /// <param name="locationEntities">Location entities list.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Validation result.</returns>
    private async Task<CustomWebResponse> ValidateDatabaseAsync(List<IndicatorsImportRow> rows, List<Location> locationEntities, CancellationToken ct = default)
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
                        ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.UpperGroupNotFound, data: upperGroup),
                    };
                }
            }
        }

        // Validate locations data
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
                        ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.LocationNotFound, data: location),
                    };
                }
            }
        }

        // Validate categories
        var baseCategoriesArray = Enum.GetValues<IndicatorBaseCategory>()
            .Select(i => (int)i)
            .ToArray();

        var predefinedCategories = await categoryRepository.GetByParentsAsync(baseCategoriesArray, ct);

        foreach (var row in rows)
        {
            if (IndicatorConstants.IndicatorsWithPredefinedCategories.Contains((IndicatorTypes)row.IndicatorTypeId))
            {
                var categoryError = !predefinedCategories.Any(e => e.Name == row.GroupName && e.Parent.Name == row.UpperGroupName);

                if (categoryError)
                {
                    return new(true)
                    {
                        ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.CategoryNotFound),
                        Message = $"Errors in row {row.RowNumber}. Value: '{row.GroupName}'",
                    };
                }
            }
        }

        return new();
    }

    #endregion

    #region Database update functions

    /// <summary>
    /// Map Category to GroupDataHelper.
    /// </summary>
    /// <param name="category">Category entity.</param>
    /// <returns>GroupDataHelper DTO.</returns>
    private static GroupDataHelper MapToHelper(Category category) =>
        new()
        {
            Id = category.Id,
            ParentId = category.ParentId,
            Name = category.Name.Trim().CapitalizeFirstOnly(),
            Description = category.Description,
            ParentName = category.Parent?.Name?.Trim()?.CapitalizeFirstOnly(),
        };

    /// <summary>
    /// Get existing categories.
    /// </summary>
    /// <param name="spreadsheetCategories">Categories from spreadsheet.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Existing categories.</returns>
    private async Task<List<GroupDataHelper>> GetExistingCategoriesAsync(GroupDataHelper[] spreadsheetCategories, CancellationToken ct = default)
    {
        var databaseCategories = (await categoryRepository.ListAsync(ct))
            .Select(MapToHelper)
            .ToArray();

        return [.. spreadsheetCategories
            .Join(databaseCategories, sc => new { sc.Name, sc.ParentName }, dbc => new { dbc.Name, dbc.ParentName }, (sc, dbc) => new { sc, dbc })
            .Select(i => i.dbc)
            .Distinct()];
    }

    /// <summary>
    /// Get existing indicator locations.
    /// </summary>
    /// <param name="indicator">Indicator entity.</param>
    /// <param name="spreadsheetLocations">Locations from spreadsheets.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Existing indicator locations.</returns>
    private async Task<List<IndicatorLocation>> GetExistingIndicatorLocationsAsync(Indicator indicator, LocationDataHelper[] spreadsheetLocations, CancellationToken ct = default)
    {
        var existingIndicatorLocations = indicator != null ? await indicatorLocationRepository.GetByIndicatorAsync(indicator?.Id ?? 0, ct) : [];

        if (existingIndicatorLocations.Count > 0)
        {
            existingIndicatorLocations = [..
                existingIndicatorLocations
                    .Where(e =>
                        spreadsheetLocations.Any(i =>
                            i.Municipality == e.Location.Name &&
                            i.Department == e.Location.Parent.Name &&
                            i.Locality == e.Locality))];
        }

        return existingIndicatorLocations;
    }

    /// <summary>
    /// Save and get categories.
    /// </summary>
    /// <param name="rows">Spreadsheet rows data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Updated categories.</returns>
    private async Task<List<GroupDataHelper>> SaveAndGetCategoriesAsync(List<IndicatorsImportRow> rows, CancellationToken ct = default)
    {
        // Get categories from spreadsheet
        var spreadsheetCategories = rows
            .Select(r => new GroupDataHelper
            {
                Name = r.GroupName,
                Description = r.GroupDescription,
                ParentName = r.UpperGroupName,
            })
            .DistinctBy(r => new { r.Name, r.Description, r.ParentName })
            .ToArray();

        // Get categories from database
        var databaseCategories = await GetExistingCategoriesAsync(spreadsheetCategories, ct);

        // Build new categories list
        var parentSpeciesCategories = await categoryRepository.GetByParentsAsync([(int)IndicatorBaseCategory.Species], ct);

        var newCategories = new List<GroupDataHelper>();

        foreach (var category in spreadsheetCategories)
        {
            var entityExists = databaseCategories.Any(i => category.Name == i.Name && category.ParentName == i.ParentName);

            if (!entityExists && !string.IsNullOrEmpty(category.Name))
            {
                category.ParentId = parentSpeciesCategories.FirstOrDefault(e => e.Name == category.ParentName)?.Id;
                newCategories.Add(category);
            }
        }

        newCategories = [.. newCategories.DistinctBy(e => new { e.ParentId, e.Name })];

        // Save categories entities
        var newCategoriesEntities = newCategories.Select(i => new Category()
        {
            ParentId = i.ParentId,
            Name = i.Name,
            Description = i.Description,
        });

        await categoryRepository.AddRangeAsync(newCategoriesEntities, ct);

        return await GetExistingCategoriesAsync(spreadsheetCategories, ct);
    }

    /// <summary>
    /// Generate IndicatorVersion entities.
    /// </summary>
    /// <param name="indicator">Indicator entity.</param>
    /// <param name="rows">Spreadsheet rows.</param>
    /// <param name="categories">Categories entities.</param>
    /// <param name="now">Current date and time.</param>
    /// <param name="indicatorLastVersion">Last indicator version.</param>
    /// <returns>IndicatorVersion entities.</returns>
    private static List<IndicatorVersion> GenerateIndicatorVersions(
        Indicator indicator,
        List<IndicatorsImportRow> rows,
        List<GroupDataHelper> categories,
        DateTimeOffset now,
        int indicatorLastVersion) =>
        [.. rows
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
                        var categoryId = categories
                            .FirstOrDefault(i =>
                                i.ParentName == g2.Key.UpperGroupName &&
                                i.Name == g2.Key.GroupName)?.Id ??
                            categories
                                .FirstOrDefault(i =>
                                    i.Name == g2.Key.UpperGroupName &&
                                    string.IsNullOrEmpty(g2.Key.GroupName))?.Id ??
                            0;

                        return new IndicatorGroup()
                        {
                            CategoryId = categoryId,
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
            })];

    /// <summary>
    /// Generate Indicator entities.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="indicator">Indicator entity.</param>
    /// <param name="rows">Spreadsheet rows.</param>
    /// <param name="indicatorVersions">IndicatorVersion entities.</param>
    /// <param name="indicatorLocations">IndicatorLocation entities.</param>
    /// <param name="locations">Location entities.</param>
    /// <param name="now">Current date and time.</param>
    /// <returns>Indicator entities.</returns>
    private static List<Indicator> GenerateIndicators(
        int initiativeId,
        Indicator indicator,
        List<IndicatorsImportRow> rows,
        List<IndicatorVersion> indicatorVersions,
        List<IndicatorLocation> indicatorLocations,
        List<Location> locations,
        DateTimeOffset now) =>
        [.. rows
            .GroupBy(r => r.IndicatorTypeId)
            .Select(g =>
            {
                var indicatorsLocations = g
                    .Select(r =>
                    {
                        IndicatorLocation indicatorLocation = null;

                        indicatorLocation = indicatorLocations
                            .FirstOrDefault(i => i.Location.Name == r.MunicipalityName && i.Location.Parent.Name == r.DepartmentName);

                        if (indicatorLocation == null)
                        {
                            var locationEntity = locations
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
                    InitiativeId = initiativeId,
                    Name = $"Indicador tipo {g.Key} ({now.ToString(GeneralConstants.DatetimeFormat, CultureInfo.CurrentCulture)})",
                    IndicatorTypeId = g.Key,
                    IndicatorLocations = indicatorsLocations,
                    Versions = [.. indicatorVersions.Where(e => e.IndicatorTypeId == g.Key)],
                };
            })];

    #endregion
}
