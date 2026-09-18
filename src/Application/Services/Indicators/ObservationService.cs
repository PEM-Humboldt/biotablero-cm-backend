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

using IndicatorTopics = Core.Domain.Utils.Enums.IndicatorsEnums.IndicatorTopic;
using IndicatorTypes = Core.Domain.Utils.Enums.IndicatorsEnums.IndicatorType;
using ObservationBaseCategory = Core.Domain.Utils.Enums.IndicatorsEnums.ObservationBaseCategory;

/// <summary>
/// Observation service.
/// </summary>
public class ObservationService : ServiceRead<Observation, ObservationDto, int>, IObservationService
{
    private new readonly IObservationRepository entityRepository;
    private readonly ILogger logger;
    private new readonly IMapperReadAndUpdate<Observation, ObservationDto> mapper;
    private readonly IValidator<ObservationDto> entityValidator;
    private readonly IIndicatorExcelService excelService;
    private readonly IInitiativeRepository initiativeRepository;
    private readonly ILocationRepository locationRepository;
    private readonly IObservationVersionRepository observationVersionRepository;
    private readonly ICategoryRepository categoryRepository;
    private readonly IObservationLocationRepository observationLocationRepository;
    private readonly IValidator<ObservationImportRow> observationImportRowValidator;
    private readonly IMapperReadAndUpdate<ObservationVersion, ObservationVersionDto> observationVersionMapper;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="entityValidator">Entity validator.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="errorTranslator">Error translator.</param>
    /// <param name="excelService">Excel service.</param>
    /// <param name="initiativeRepository">Initiative repository.</param>
    /// <param name="locationRepository">Location repository.</param>
    /// <param name="observationVersionRepository">Observation version repository.</param>
    /// <param name="categoryRepository">Observation Category repository.</param>
    /// <param name="observationLocationRepository">Observation Location repository.</param>
    /// <param name="observationImportRowValidator">Observations spreadsheet row validator.</param>
    /// <param name="observationVersionMapper">Observation version mapper.</param>
    public ObservationService(
        IObservationRepository entityRepository,
        IValidator<ObservationDto> entityValidator,
        ILogger logger,
        IMapperReadAndUpdate<Observation, ObservationDto> mapper,
        IValidationErrorTranslator errorTranslator,
        IIndicatorExcelService excelService,
        IInitiativeRepository initiativeRepository,
        ILocationRepository locationRepository,
        IObservationVersionRepository observationVersionRepository,
        ICategoryRepository categoryRepository,
        IObservationLocationRepository observationLocationRepository,
        IValidator<ObservationImportRow> observationImportRowValidator,
        IMapperReadAndUpdate<ObservationVersion, ObservationVersionDto> observationVersionMapper)
    : base(entityRepository, mapper, errorTranslator)
    {
        this.entityRepository = entityRepository;
        this.entityValidator = entityValidator;
        this.logger = logger;
        this.mapper = mapper;
        this.excelService = excelService;
        this.initiativeRepository = initiativeRepository;
        this.locationRepository = locationRepository;
        this.observationVersionRepository = observationVersionRepository;
        this.categoryRepository = categoryRepository;
        this.observationLocationRepository = observationLocationRepository;
        this.observationImportRowValidator = observationImportRowValidator;
        this.observationVersionMapper = observationVersionMapper;
    }

    /// <inheritdoc/>
    public override async Task<CustomWebResponse> GetListAsync(ODataQueryOptions<Observation> queryOptions, CancellationToken ct = default)
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
    public async Task<CustomWebResponse> UpdateAsync(int id, ObservationDto entityData, CancellationToken ct = default)
    {
        // Validate data
        var validationResult = await entityValidator.ValidateAsync(entityData, ct);

        if (!validationResult.IsValid)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(validationResult.Errors),
            };
        }

        // Validate entity
        var entity = await entityRepository.GetByIdAsync(id, ct);

        if (entity == null)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.ElementNotFound),
            };
        }

        // Update entity data
        mapper.Update(entity, entityData);

        await entityRepository.UpdateAsync(entity, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Update, "Updated observation", "{@EntityData}", entityData);

        return new()
        {
            ResponseBody = entityData,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> ImportObservationsAsync(string? userName, ObservationsImportFileDto requestData, IInputFile formFile, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(userName);

        var result = new SpreadsheetUploadResult()
        {
            DoNotModifyDatabase = requestData.DoNotModifyDatabase,
        };

        // Validate file
        if (formFile.IsEmpty())
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Files.Empty),
            };
        }

        if (!formFile.ItIsAValidSpreadsheet())
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Files.InvalidFormat),
            };
        }

        // Validate spreadsheet structure
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

        // Validate observation
        Observation? observation = null;
        var observationLastVersion = 1;

        if (requestData.Id.HasValue)
        {
            observation = await entityRepository.GetByIdAsync(requestData.Id.Value, ct);

            if (observation == null)
            {
                return new(true)
                {
                    ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.NotFound),
                };
            }

            observationLastVersion = await observationVersionRepository.GetLastVersionAsync(observation.Id, ct);
        }

        // Adjust spreadsheet rows data
        AdjustRowsData(fileReadResult.Rows);

        // Make structure validations
        var structureDataValidations = await ValidateStructureDataAsync(fileReadResult.Rows, observation, ct);

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
            spreadsheetLocations?.Select(e => e.Department)?.ToArray() ?? [],
            spreadsheetLocations?.Select(e => e.Municipality)?.ToArray() ?? [],
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

            var observationLocations = await GetExistingObservationLocationsAsync(observation, spreadsheetLocations ?? [], ct);
            var categories = await SaveAndGetCategoriesAsync(fileReadResult.Rows, ct);
            var observationVersionEntities = GenerateObservationVersions(observation, fileReadResult.Rows, categories, now, observationLastVersion);

            if (!requestData.Id.HasValue)
            {
                var observations = GenerateObservations(requestData.InitiativeId, observation, fileReadResult.Rows, observationVersionEntities, observationLocations, locationEntities, now);

                // Save data
                await entityRepository.AddRangeAsync(observations, ct);

                var observationDtos = observations
                    .Select(mapper.Map)
                    .ToList();

                logger.AddLog(LogType.Create, "Added observations", "{@EntityData}", observationDtos);

                result.Result = observationDtos;
            }
            else
            {
                // Save data
                await observationVersionRepository.AddRangeAsync(observationVersionEntities, ct);

                var observationVersionDtos = observationVersionEntities
                    .Select(observationVersionMapper.Map);

                logger.AddLog(LogType.Create, "Added observation versions", "{@EntityData}", observationVersionDtos);

                result.Result = observationVersionDtos;
            }
        }

        result.SuccessfulProcess = true;
        return new()
        {
            ResponseBody = result,
        };
    }

    #region Import Observation functions

    /// <summary>
    /// Cast observation value date.
    /// </summary>
    /// <param name="year">Observation value year.</param>
    /// <param name="month">Observation value month.</param>
    /// <returns>DateTime from strings.</returns>
    /// <exception cref="InvalidCastException">Cast date error.</exception>
    private static DateTime? CastDate(string year, string month)
    {
        if (string.IsNullOrEmpty(year) || string.IsNullOrEmpty(month))
        {
            return null;
        }

        var formattedMonth = month?.PadLeft(2, '0');
        var parseSuccessful = DateTime.TryParseExact(
            string.Format(
                GeneralConstants.DefaultFormatProvider,
                IndicatorConstants.ObservationDateFormat,
                year,
                formattedMonth),
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
    private static void AdjustRowsData(List<ObservationImportRow> rows)
    {
        foreach (var row in rows)
        {
            row.ObservationName = row.ObservationName?.Trim()?.CapitalizeFirstOnly()!;
            row.UpperGroupName = row.UpperGroupName?.Trim()?.CapitalizeFirstOnly()!;
            row.GroupName = row.GroupName?.Trim()?.CapitalizeFirstOnly();
            row.LocalityName = row.LocalityName?.Trim()?.CapitalizeFirstOnly()!;

            if (row.IndicatorTopicId == (int)IndicatorTopics.SpeciesDiversity)
            {
                row.GroupName = row.UpperGroupName;
                row.UpperGroupName = IndicatorConstants.SpeciesCategoryName;
            }
        }

        rows.RemoveAll(r => r.GroupName?.Equals(IndicatorConstants.TotalGroupName, StringComparison.OrdinalIgnoreCase) == true);
    }

    /// <summary>
    /// Spreadsheet structure data validations.
    /// </summary>
    /// <param name="rows">Spreadsheet rows.</param>
    /// <param name="observation">Observation (optional).</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Validation result.</returns>
    private async Task<CustomWebResponse> ValidateStructureDataAsync(List<ObservationImportRow> rows, Observation? observation, CancellationToken ct = default)
    {
        var totalObservations = rows
            .GroupBy(r => r.ObservationName)
            .Count();

        // Validate total observations for edition
        if (observation != null)
        {
            if (totalObservations != 1)
            {
                return new(true)
                {
                    ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.OnlyOneObservationRequired),
                };
            }
        }

        // Validate unique indicator topic by observations grouped by name
        var totalTopics = rows
            .GroupBy(r => r.ObservationName)
            .SelectMany(g => g.Select(r => r.IndicatorTopicId).Distinct())
            .Count();

        if (totalObservations != totalTopics)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.ObservationsAndTopicsDifference),
                Message = $"Observations: {totalObservations}. Topics: {totalTopics}",
            };
        }

        foreach (var row in rows)
        {
            // Check FluentValidation validations
            var validationResult = await observationImportRowValidator.ValidateAsync(row, ct);

            if (!validationResult.IsValid)
            {
                return new(true)
                {
                    ResponseBody = errorTranslator.Translate(validationResult.Errors),
                    Message = $"Errors in row {row.RowNumber}",
                };
            }

            // Check observation topics
            if (!Enum.GetValues<IndicatorTopics>().Select(e => (int)e).Contains(row.IndicatorTopicId))
            {
                return new(true)
                {
                    ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.InvalidIndicatorTopic),
                    Message = $"Errors in row {row.RowNumber}",
                };
            }

            // Check observation measure units
            foreach (var indicatorType in IndicatorConstants.IndicatorTypesByIndicatorTopic)
            {
                if (row.IndicatorTopicId == (int)indicatorType.Key && !indicatorType.Value.Contains((IndicatorTypes)row.IndicatorTypeId))
                {
                    return new(true)
                    {
                        ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.InvalidIndicatorType),
                        Message = $"Errors in row {row.RowNumber}",
                    };
                }
            }

            // Check observations with species
            if (IndicatorConstants.ObservationsWithSpecies.Contains((IndicatorTopics)row.IndicatorTopicId))
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

            // Check observations with integer values
            if (IndicatorConstants.ObservationsWithIntegerValues.Contains((IndicatorTopics)row.IndicatorTopicId))
            {
                if (row.Value % 1 != 0)
                {
                    return new(true)
                    {
                        ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.InvalidIntegerValue, data: row.Value),
                        Message = $"Errors in row {row.RowNumber}",
                    };
                }
            }

            // Check observations with confidence interval
            if (IndicatorConstants.ObservationsWithConfidenceInterval.Contains((IndicatorTopics)row.IndicatorTopicId))
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

            // Check observations with date ranges
            if (IndicatorConstants.ObservationsWithDateRange.Contains((IndicatorTopics)row.IndicatorTopicId))
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
                // Check observations without date ranges
                if (row.FinalYear != null || row.FinalMonth != null)
                {
                    return new(true)
                    {
                        ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.FinalDateNotRequired),
                        Message = $"Errors in row {row.RowNumber}",
                    };
                }
            }

            // Validations for observations editions
            if (observation != null)
            {
                if (row.IndicatorTopicId != observation.IndicatorTopicId)
                {
                    return new(true)
                    {
                        ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.InvalidIndicatorTopic),
                        Message = $"Errors in row {row.RowNumber}. Original type id: {observation.IndicatorTopicId}, Spreadsheet type id: {row.IndicatorTopicId}",
                    };
                }

                if (!observation?.ObservationLocations?.Any(e => e.Locality == row.LocalityName && e.Location?.Name == row.MunicipalityName && row.DepartmentName == e.Location?.Parent?.Name) ?? false)
                {
                    return new(true)
                    {
                        ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.InvalidLocationData),
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
    private async Task<CustomWebResponse> ValidateDatabaseAsync(List<ObservationImportRow> rows, List<Location> locationEntities, CancellationToken ct = default)
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

        locationEntities = [.. locationEntities.Where(e => spreadsheetLocations.Any(i => i.Municipality == e.Name && i.Department == e.Parent?.Name))];

        if (spreadsheetLocations.Length != locationEntities.Count)
        {
            var locationEntitiesKeyValuePairs = locationEntities
                .Select(e => new LocationDataHelper
                {
                    Department = e.Parent?.Name,
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
        var baseCategoriesArray = Enum.GetValues<ObservationBaseCategory>()
            .Select(i => (int)i)
            .ToArray();

        var predefinedCategories = await categoryRepository.GetByParentsAsync(baseCategoriesArray, ct);

        foreach (var row in rows)
        {
            if (IndicatorConstants.ObservationsWithPredefinedCategories.Contains((IndicatorTopics)row.IndicatorTopicId))
            {
                var categoryError = !predefinedCategories.Any(e => e.Name == row.GroupName && e.Parent?.Name == row.UpperGroupName);

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
    /// Get existing observation locations.
    /// </summary>
    /// <param name="observation">Observation entity.</param>
    /// <param name="spreadsheetLocations">Locations from spreadsheets.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Existing observation locations.</returns>
    private async Task<List<ObservationLocation>> GetExistingObservationLocationsAsync(Observation? observation, LocationDataHelper[] spreadsheetLocations, CancellationToken ct = default)
    {
        var existingObservationLocations = observation != null ? await observationLocationRepository.GetByObservationAsync(observation?.Id ?? 0, ct) : [];

        if (existingObservationLocations.Count > 0)
        {
            existingObservationLocations = [..
                existingObservationLocations
                    .Where(e =>
                        spreadsheetLocations.Any(i =>
                            i.Municipality == e.Location?.Name &&
                            i.Department == e.Location?.Parent?.Name &&
                            i.Locality == e.Locality))];
        }

        return existingObservationLocations;
    }

    /// <summary>
    /// Save and get categories.
    /// </summary>
    /// <param name="rows">Spreadsheet rows data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Updated categories.</returns>
    private async Task<List<GroupDataHelper>> SaveAndGetCategoriesAsync(List<ObservationImportRow> rows, CancellationToken ct = default)
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
        var parentSpeciesCategories = await categoryRepository.GetByParentsAsync([(int)ObservationBaseCategory.Species], ct);

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
            Name = i.Name!,
            Description = i.Description,
        });

        await categoryRepository.AddRangeAsync(newCategoriesEntities, ct);

        return await GetExistingCategoriesAsync(spreadsheetCategories, ct);
    }

    /// <summary>
    /// Generate ObservationVersion entities.
    /// </summary>
    /// <param name="observation">Observation entity.</param>
    /// <param name="rows">Spreadsheet rows.</param>
    /// <param name="categories">Categories entities.</param>
    /// <param name="now">Current date and time.</param>
    /// <param name="observationLastVersion">Last observation version.</param>
    /// <returns>ObservationVersion entities.</returns>
    private static List<ObservationVersion> GenerateObservationVersions(
        Observation? observation,
        List<ObservationImportRow> rows,
        List<GroupDataHelper> categories,
        DateTimeOffset now,
        int observationLastVersion) =>
        [.. rows
            .GroupBy(r => r.ObservationName)
            .Select(g => new ObservationVersion()
            {
                IndicatorTopicId = g.Select(r => r.IndicatorTopicId).FirstOrDefault(),
                ObservationId = observation?.Id ?? 0,
                ObservationName = g.Key,
                CreationDate = now,
                Version = observation == null ? 1 : observationLastVersion + 1,
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

                        return new ObservationGroup()
                        {
                            CategoryId = categoryId,
                            Values = [.. g2.Select(g2r =>
                            {
                                return new IndicatorValue()
                                {
                                    IndicatorTypeId = g2r.IndicatorTypeId,
                                    Date = CastDate(g2r.Year, g2r.Month) ?? default,
                                    DateEnd = CastDate(g2r.FinalYear!, g2r.FinalMonth!),
                                    Value = g2r.Value,
                                    UpperLimit = g2r.UpperLimit,
                                    LowerLimit = g2r.LowerLimit,
                                };
                            })],
                        };
                    })],
            })];

    /// <summary>
    /// Generate Observation entities.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="observation">Observation entity.</param>
    /// <param name="rows">Spreadsheet rows.</param>
    /// <param name="observationVersions">ObservationVersion entities.</param>
    /// <param name="observationLocations">ObservationLocation entities.</param>
    /// <param name="locations">Location entities.</param>
    /// <param name="now">Current date and time.</param>
    /// <returns>Observation entities.</returns>
    private static List<Observation> GenerateObservations(
        int initiativeId,
        Observation? observation,
        List<ObservationImportRow> rows,
        List<ObservationVersion> observationVersions,
        List<ObservationLocation> observationLocations,
        List<Location> locations,
        DateTimeOffset now) =>
        [.. rows
            .GroupBy(r => r.ObservationName)
            .Select(g =>
            {
                var observationsLocations = g
                    .Select(r =>
                    {
                        ObservationLocation? observationLocation = null;

                        observationLocation = observationLocations
                            .FirstOrDefault(i => i.Location?.Name == r.MunicipalityName && i.Location?.Parent?.Name == r.DepartmentName);

                        if (observationLocation == null)
                        {
                            var locationEntity = locations
                                .FirstOrDefault(i => i.Name == r.MunicipalityName && i.Parent?.Name == r.DepartmentName);

                            observationLocation = new ObservationLocation()
                            {
                                ObservationId = observation?.Id ?? 0,
                                LocationId = locationEntity?.Id ?? 0,
                                Location = locationEntity,
                                Locality = r.LocalityName,
                            };
                        }

                        return observationLocation;
                    })
                    .DistinctBy(e => new { e.Id, e.LocationId })
                    .ToList();

                var indicatorTopicId = g.Select(r => r.IndicatorTopicId).FirstOrDefault();

                return new Observation()
                {
                    InitiativeId = initiativeId,
                    Name = g.Key,
                    IndicatorTopicId = indicatorTopicId,
                    ObservationLocations = observationsLocations,
                    Versions = [.. observationVersions.Where(e => e.IndicatorTopicId == indicatorTopicId && e.ObservationName == g.Key)],
                };
            })];

    #endregion
}
