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

using IndicatorTypes = Core.Domain.Utils.Enums.IndicatorsEnums.IndicatorType;
using TopicBaseCategory = Core.Domain.Utils.Enums.IndicatorsEnums.TopicBaseCategory;
using TopicTypes = Core.Domain.Utils.Enums.IndicatorsEnums.TopicType;

/// <summary>
/// Topic service.
/// </summary>
public class TopicService : ServiceRead<Topic, TopicDto, int>, ITopicService
{
    private new readonly ITopicRepository entityRepository;
    private readonly ILogger logger;
    private new readonly IMapperReadAndUpdate<Topic, TopicDto> mapper;
    private readonly IValidator<TopicDto> entityValidator;
    private readonly IIndicatorExcelService excelService;
    private readonly IInitiativeRepository initiativeRepository;
    private readonly ILocationRepository locationRepository;
    private readonly ITopicVersionRepository topicVersionRepository;
    private readonly ICategoryRepository categoryRepository;
    private readonly ITopicLocationRepository topicLocationRepository;
    private readonly ICentralActorRepository centralActorRepository;
    private readonly IValidator<TopicImportRow> topicImportRowValidator;
    private readonly IMapperReadAndUpdate<TopicVersion, TopicVersionDto> topicVersionMapper;

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
    /// <param name="topicVersionRepository">Topic version repository.</param>
    /// <param name="categoryRepository">Topic Category repository.</param>
    /// <param name="topicLocationRepository">Topic Location repository.</param>
    /// <param name="centralActorRepository">Central actor repository.</param>
    /// <param name="topicImportRowValidator">Topics spreadsheet row validator.</param>
    /// <param name="topicVersionMapper">Topic version mapper.</param>
    public TopicService(
        ITopicRepository entityRepository,
        IValidator<TopicDto> entityValidator,
        ILogger logger,
        IMapperReadAndUpdate<Topic, TopicDto> mapper,
        IValidationErrorTranslator errorTranslator,
        IIndicatorExcelService excelService,
        IInitiativeRepository initiativeRepository,
        ILocationRepository locationRepository,
        ITopicVersionRepository topicVersionRepository,
        ICategoryRepository categoryRepository,
        ITopicLocationRepository topicLocationRepository,
        ICentralActorRepository centralActorRepository,
        IValidator<TopicImportRow> topicImportRowValidator,
        IMapperReadAndUpdate<TopicVersion, TopicVersionDto> topicVersionMapper)
    : base(entityRepository, mapper, errorTranslator)
    {
        this.entityRepository = entityRepository;
        this.entityValidator = entityValidator;
        this.logger = logger;
        this.mapper = mapper;
        this.excelService = excelService;
        this.initiativeRepository = initiativeRepository;
        this.locationRepository = locationRepository;
        this.topicVersionRepository = topicVersionRepository;
        this.categoryRepository = categoryRepository;
        this.topicLocationRepository = topicLocationRepository;
        this.centralActorRepository = centralActorRepository;
        this.topicImportRowValidator = topicImportRowValidator;
        this.topicVersionMapper = topicVersionMapper;
    }

    /// <inheritdoc/>
    public override async Task<CustomWebResponse> GetListAsync(ODataQueryOptions<Topic> queryOptions, CancellationToken ct = default)
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
    public async Task<CustomWebResponse> UpdateAsync(int id, TopicDto entityData, CancellationToken ct = default)
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

        // Validate duplicated entities
        var hasDuplicatedEntities = await entityRepository.IsDuplicatedAsync(id, entity.InitiativeId, entityData.Name, ct);

        if (hasDuplicatedEntities)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.Duplicated),
            };
        }

        // Update entity data
        mapper.Update(entity, entityData);

        await entityRepository.UpdateAsync(entity, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Update, "Updated topic", "{@EntityData}", entityData);

        return new()
        {
            ResponseBody = entityData,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> ImportTopicsAsync(string? userName, TopicsImportFileDto requestData, IInputFile formFile, CancellationToken ct = default)
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
        if (requestData.InitiativeId == InitiativeConstants.AllyRoleDefaultInitiative)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Initiatives.Disabled),
            };
        }

        if (!await initiativeRepository.AnyAsync(requestData.InitiativeId, ct))
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Initiatives.NotFound),
            };
        }

        // Validate existent topics
        var topicNames = fileReadResult.Rows
            .Select(r => r.TopicName)
            .ToArray();

        var existentTopics = await entityRepository.GetByInitiativeAndNamesAsync(requestData.InitiativeId, topicNames, ct);

        foreach (var row in fileReadResult.Rows)
        {
            row.TopicEntity = existentTopics
                .FirstOrDefault(e => e.Name == row.TopicName);
        }

        // Adjust spreadsheet rows data
        AdjustRowsData(fileReadResult.Rows);

        // Make structure validations
        var structureDataValidations = await ValidateStructureDataAsync(fileReadResult.Rows, ct);

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

        var databaseValidations = await ValidateDatabaseAsync(requestData.InitiativeId, fileReadResult.Rows, locationEntities, ct);

        if (!databaseValidations.Success)
        {
            return databaseValidations;
        }

        // Prepare data
        var newTopics = new List<Topic>();
        var editedTopics = new List<TopicVersion>();
        var now = DateTimeOffset.Now;
        var modifyDatabase = !requestData.DoNotModifyDatabase;
        var groupedRows = fileReadResult.Rows
            .GroupBy(r => r.TopicEntity);

        foreach (var group in groupedRows)
        {
            var topicLocations = await GetExistingTopicLocationsAsync(group.Key, spreadsheetLocations ?? [], ct);
            var categories = modifyDatabase ? await SaveAndGetCategoriesAsync([.. group], ct) : [];
            var centralActors = modifyDatabase ? await SaveAndGetCentralActorsAsync([.. group], ct) : [];
            var topicVersionEntities = GenerateTopicVersions(group.Key, [.. group], categories, centralActors, now, group.Key?.LastVersion ?? 1);

            if (group.Key == null)
            {
                var topics = GenerateTopics(requestData.InitiativeId, [.. group], topicVersionEntities, topicLocations, locationEntities, now);

                newTopics.AddRange(topics);
            }
            else
            {
                editedTopics.AddRange(topicVersionEntities);
            }
        }

        result.Description = new()
        {
            NewTopics = [.. newTopics.Select(e => e.Name)],
            EditedTopics = [.. editedTopics.Select(e => e.TopicName ?? string.Empty)],
        };

        // Save data
        if (modifyDatabase)
        {
            // Save new topics
            if (newTopics.Count > 0)
            {
                await entityRepository.AddRangeAsync(newTopics, ct);

                var topicDtos = newTopics
                    .Select(mapper.Map)
                    .ToList();

                logger.AddLog(LogType.Create, "Added topics", "{@EntityData}", topicDtos);
            }

            // Update topics
            if (editedTopics.Count > 0)
            {
                await topicVersionRepository.AddRangeAsync(editedTopics, ct);

                var topicVersionDtos = editedTopics
                    .Select(topicVersionMapper.Map);

                logger.AddLog(LogType.Create, "Added topic versions", "{@EntityData}", topicVersionDtos);
            }
        }

        result.SuccessfulProcess = true;
        return new()
        {
            ResponseBody = result,
        };
    }

    #region Import Topic functions

    /// <summary>
    /// Cast topic value date.
    /// </summary>
    /// <param name="year">Topic value year.</param>
    /// <param name="month">Topic value month.</param>
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
                IndicatorConstants.TopicDateFormat,
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
    private static void AdjustRowsData(List<TopicImportRow> rows)
    {
        foreach (var row in rows)
        {
            row.TopicName = row.TopicName?.Trim()?.CapitalizeFirstOnly()!;
            row.UpperGroupName = row.UpperGroupName?.Trim()?.CapitalizeFirstOnly()!;
            row.GroupName = row.GroupName?.Trim()?.CapitalizeFirstOnly();
            row.LocalityName = row.LocalityName?.Trim()?.CapitalizeFirstOnly()!;
            row.CentralActorName = row.CentralActorName?.Trim()?.CapitalizeFirstOnly()!;

            if (row.TopicTypeId == (int)TopicTypes.SpeciesDiversity)
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
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Validation result.</returns>
    private async Task<CustomWebResponse> ValidateStructureDataAsync(List<TopicImportRow> rows, CancellationToken ct = default)
    {
        var totalTopics = rows
            .GroupBy(r => r.TopicName)
            .Count();

        // Validate unique topic type by topics grouped by name
        var totalTopicTypes = rows
            .GroupBy(r => r.TopicName)
            .SelectMany(g => g.Select(r => r.TopicTypeId).Distinct())
            .Count();

        if (totalTopics != totalTopicTypes)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.TopicsAndTypesDifference),
                Message = $"Topics: {totalTopics}. Topic types: {totalTopicTypes}",
            };
        }

        foreach (var row in rows)
        {
            // Check FluentValidation validations
            var validationResult = await topicImportRowValidator.ValidateAsync(row, ct);

            if (!validationResult.IsValid)
            {
                return new(true)
                {
                    ResponseBody = errorTranslator.Translate(validationResult.Errors),
                    Message = $"Errors in row {row.RowNumber}",
                };
            }

            // Check topic types
            if (!Enum.GetValues<TopicTypes>().Select(e => (int)e).Contains(row.TopicTypeId))
            {
                return new(true)
                {
                    ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.InvalidTopicType),
                    Message = $"Errors in row {row.RowNumber}",
                };
            }

            // Check indicator types
            foreach (var indicatorType in IndicatorConstants.IndicatorTypesByTopicType)
            {
                if (row.TopicTypeId == (int)indicatorType.Key && !indicatorType.Value.Contains((IndicatorTypes)row.IndicatorTypeId))
                {
                    return new(true)
                    {
                        ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.InvalidIndicatorType),
                        Message = $"Errors in row {row.RowNumber}",
                    };
                }
            }

            // Check topics with species
            if (IndicatorConstants.TopicsWithSpecies.Contains((TopicTypes)row.TopicTypeId))
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

            // Check topics with integer values
            if (IndicatorConstants.TopicsWithIntegerValues.Contains((TopicTypes)row.TopicTypeId))
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

            // Check topics with confidence interval
            if (IndicatorConstants.TopicsWithConfidenceInterval.Contains((TopicTypes)row.TopicTypeId))
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

            // Check topics with date ranges
            if (IndicatorConstants.TopicsWithDateRange.Contains((TopicTypes)row.TopicTypeId))
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
                // Check topics without date ranges
                if (row.FinalYear != null || row.FinalMonth != null)
                {
                    return new(true)
                    {
                        ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.FinalDateNotRequired),
                        Message = $"Errors in row {row.RowNumber}",
                    };
                }
            }

            // Check topics with central actors
            if (IndicatorConstants.TopicsWithCentralActors.Contains((TopicTypes)row.TopicTypeId))
            {
                if (string.IsNullOrEmpty(row.CentralActorName))
                {
                    return new(true)
                    {
                        ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.CentralActorRequired),
                        Message = $"Errors in row {row.RowNumber}",
                    };
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(row.CentralActorName))
                {
                    return new(true)
                    {
                        ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.CentralActorNotRequired),
                        Message = $"Errors in row {row.RowNumber}",
                    };
                }
            }

            // Validations for topics editions
            if (row.TopicEntity != null)
            {
                if (row.TopicTypeId != row.TopicEntity.TopicTypeId)
                {
                    return new(true)
                    {
                        ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.InvalidTopicType),
                        Message = $"Errors in row {row.RowNumber}. Original type id: {row.TopicEntity.TopicTypeId}, Spreadsheet type id: {row.TopicTypeId}",
                    };
                }

                if (!row.TopicEntity?.TopicLocations?.Any(e => e.Locality == row.LocalityName && e.Location?.Name == row.MunicipalityName && row.DepartmentName == e.Location?.Parent?.Name) ?? false)
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
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="rows">Spreadsheet rows.</param>
    /// <param name="locationEntities">Location entities list.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Validation result.</returns>
    private async Task<CustomWebResponse> ValidateDatabaseAsync(int initiativeId, List<TopicImportRow> rows, List<Location> locationEntities, CancellationToken ct = default)
    {
        var newTopicNames = rows
            .Where(r => r.TopicEntity == null)
            .Select(r => r.TopicName)
            .Distinct()
            .ToArray();

        if (newTopicNames.Length > 0)
        {
            var existentTopics = await entityRepository.GetByInitiativeAndNamesAsync(initiativeId, newTopicNames, ct);

            if (existentTopics.Any())
            {
                return new(true)
                {
                    ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Indicators.DuplicatedElements, data: existentTopics),
                };
            }
        }

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
        var baseCategoriesArray = Enum.GetValues<TopicBaseCategory>()
            .Select(i => (int)i)
            .ToArray();

        var predefinedCategories = await categoryRepository.GetByParentsAsync(baseCategoriesArray, ct);

        foreach (var row in rows)
        {
            if (IndicatorConstants.TopicsWithPredefinedCategories.Contains((TopicTypes)row.TopicTypeId))
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
    /// Get existing topic locations.
    /// </summary>
    /// <param name="topic">Topic entity.</param>
    /// <param name="spreadsheetLocations">Locations from spreadsheets.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Existing topic locations.</returns>
    private async Task<List<TopicLocation>> GetExistingTopicLocationsAsync(Topic? topic, LocationDataHelper[] spreadsheetLocations, CancellationToken ct = default)
    {
        var existingTopicLocations = topic != null ? await topicLocationRepository.GetByTopicAsync(topic?.Id ?? 0, ct) : [];

        if (existingTopicLocations.Count > 0)
        {
            existingTopicLocations = [..
                existingTopicLocations
                    .Where(e =>
                        spreadsheetLocations.Any(i =>
                            i.Municipality == e.Location?.Name &&
                            i.Department == e.Location?.Parent?.Name &&
                            i.Locality == e.Locality))];
        }

        return existingTopicLocations;
    }

    /// <summary>
    /// Save and get categories.
    /// </summary>
    /// <param name="rows">Spreadsheet rows data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Updated categories.</returns>
    private async Task<List<GroupDataHelper>> SaveAndGetCategoriesAsync(List<TopicImportRow> rows, CancellationToken ct = default)
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
        var parentSpeciesCategories = await categoryRepository.GetByParentsAsync([(int)TopicBaseCategory.Species], ct);

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
    /// Save and get central actors.
    /// </summary>
    /// <param name="rows">Spreadsheet rows data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Updated central actors.</returns>
    private async Task<List<CentralActor>> SaveAndGetCentralActorsAsync(List<TopicImportRow> rows, CancellationToken ct = default)
    {
        // Get central actors from spreadsheet
        var spreadsheetCentralActors = rows
            .Where(r => r.CentralActorName != null)
            .Select(r => r.CentralActorName!)
            .Distinct()
            .ToArray();

        // Get central actors from database
        var databaseCentralActors = await centralActorRepository.GetByNamesAsync(spreadsheetCentralActors, ct);

        // Build new central actors list
        var newCentralActors = new List<CentralActor>();

        foreach (var centralActorName in spreadsheetCentralActors)
        {
            var entityExists = databaseCentralActors.Any(e => centralActorName == e.Name);

            if (!entityExists && !string.IsNullOrEmpty(centralActorName))
            {
                newCentralActors.Add(new()
                {
                    Name = centralActorName,
                });
            }
        }

        newCentralActors = [.. newCentralActors.Distinct()];

        await centralActorRepository.AddRangeAsync(newCentralActors, ct);

        return [.. await centralActorRepository.GetByNamesAsync(spreadsheetCentralActors, ct)];
    }

    /// <summary>
    /// Generate TopicVersion entities.
    /// </summary>
    /// <param name="topic">Topic entity.</param>
    /// <param name="rows">Spreadsheet rows.</param>
    /// <param name="categories">Categories entities.</param>
    /// <param name="centralActors">Central actors entities.</param>
    /// <param name="now">Current date and time.</param>
    /// <param name="topicLastVersion">Last topic version.</param>
    /// <returns>TopicVersion entities.</returns>
    private static List<TopicVersion> GenerateTopicVersions(
        Topic? topic,
        List<TopicImportRow> rows,
        List<GroupDataHelper> categories,
        List<CentralActor> centralActors,
        DateTimeOffset now,
        int topicLastVersion) =>
        [.. rows
            .GroupBy(r => r.TopicName)
            .Select(g => new TopicVersion()
            {
                IndicatorTopicId = g.Select(r => r.TopicTypeId).FirstOrDefault(),
                TopicId = topic?.Id ?? 0,
                TopicName = g.Key,
                CreationDate = now,
                Version = topic == null ? 1 : topicLastVersion + 1,
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

                        return new TopicGroup()
                        {
                            CategoryId = categoryId,
                            Values = [.. g2.Select(g2r =>
                            {
                                var centralActorId = centralActors
                                    .FirstOrDefault(i => i.Name == g2r.CentralActorName)?.Id;

                                return new IndicatorValue()
                                {
                                    IndicatorTypeId = g2r.IndicatorTypeId,
                                    CentralActorId = centralActorId,
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
    /// Generate Topic entities.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="rows">Spreadsheet rows.</param>
    /// <param name="topicVersions">TopicVersion entities.</param>
    /// <param name="topicLocations">TopicLocation entities.</param>
    /// <param name="locations">Location entities.</param>
    /// <param name="now">Current date and time.</param>
    /// <returns>Topic entities.</returns>
    private static List<Topic> GenerateTopics(
        int initiativeId,
        List<TopicImportRow> rows,
        List<TopicVersion> topicVersions,
        List<TopicLocation> topicLocations,
        List<Location> locations,
        DateTimeOffset now) =>
        [.. rows
            .GroupBy(r => r.TopicName)
            .Select(g =>
            {
                var topicsLocations = g
                    .Select(r =>
                    {
                        TopicLocation? topicLocation = null;

                        topicLocation = topicLocations
                            .FirstOrDefault(i => i.Location?.Name == r.MunicipalityName && i.Location?.Parent?.Name == r.DepartmentName);

                        if (topicLocation == null)
                        {
                            var locationEntity = locations
                                .FirstOrDefault(i => i.Name == r.MunicipalityName && i.Parent?.Name == r.DepartmentName);

                            topicLocation = new TopicLocation()
                            {
                                TopicId = r.TopicEntity?.Id ?? 0,
                                LocationId = locationEntity?.Id ?? 0,
                                Location = locationEntity,
                                Locality = r.LocalityName,
                            };
                        }

                        return topicLocation;
                    })
                    .DistinctBy(e => new { e.Id, e.LocationId })
                    .ToList();

                var indicatorTopicId = g.Select(r => r.TopicTypeId).FirstOrDefault();

                return new Topic()
                {
                    InitiativeId = initiativeId,
                    Name = g.Key,
                    TopicTypeId = indicatorTopicId,
                    TopicLocations = topicsLocations,
                    Versions = [.. topicVersions.Where(e => e.IndicatorTopicId == indicatorTopicId && e.TopicName == g.Key)],
                };
            })];

    #endregion
}
