namespace IAVH.BioTablero.CM.Application.Services.Initiatives;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Geo;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Initiatives;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Models.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;
using IAVH.BioTablero.CM.Core.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Locations;

using Microsoft.AspNetCore.OData.Query;

using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;
using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

using InitiativeUserLevelEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.InitiativeUserLevel;

/// <summary>
/// Initiative service.
/// </summary>
public class InitiativeService : ServiceRead<Initiative, InitiativeDto, int>, IInitiativeService
{
    private const int MaxLeadersByInitiative = 3;
    private const string StoragePrefix = "initiatives";
    private readonly IValidator<InitiativeDto> entityValidator;
    private readonly ILogger logger;
    private new readonly IMapperCreateReadAndUpdate<Initiative, InitiativeDto> mapper;
    private new readonly IInitiativeRepository entityRepository;
    private readonly ILocationRepository locationRepository;
    private readonly ILocationService locationService;
    private readonly IIamService iamService;
    private readonly IStorageService storageService;
    private readonly IImageUtilsService imageUtilsService;
    private readonly GeoJsonWriter geoJsonWriter;
    private readonly GeoJsonReader geoJsonReader;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="errorTranslator">Error translator.</param>
    /// <param name="entityValidator">Entity validator.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="locationRepository">Initiative Location repository.</param>
    /// <param name="locationService">Location service.</param>
    /// <param name="iamService">IAM service.</param>
    /// <param name="storageService">Storage service.</param>
    /// <param name="imageUtilsService">Image utils service.</param>
    public InitiativeService(
        IInitiativeRepository entityRepository,
        IMapperCreateReadAndUpdate<Initiative, InitiativeDto> mapper,
        IValidationErrorTranslator errorTranslator,
        IValidator<InitiativeDto> entityValidator,
        ILogger logger,
        ILocationRepository locationRepository,
        ILocationService locationService,
        IIamService iamService,
        IStorageService storageService,
        IImageUtilsService imageUtilsService)
        : base(entityRepository, mapper, errorTranslator)
    {
        this.entityRepository = entityRepository;
        this.mapper = mapper;
        this.entityValidator = entityValidator;
        this.logger = logger;
        this.locationRepository = locationRepository;
        this.locationService = locationService;
        this.iamService = iamService;
        this.storageService = storageService;
        this.imageUtilsService = imageUtilsService;

        geoJsonWriter = new GeoJsonWriter();
        geoJsonReader = new GeoJsonReader();
    }

    /// <inheritdoc/>
    public override async Task<CustomWebResponse> GetListAsync(ODataQueryOptions<Initiative> queryOptions, CancellationToken ct = default)
    {
        var query = entityRepository.GetQueryable();
        query = entityRepository.IncludeOdataEntities(query);

        return await GetOdataListByQueryAsync(query, queryOptions, ct);
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetByUserNameAsync(string userName, CancellationToken ct = default)
    {
        var dataListEntity = await entityRepository.GetByUserNameAsync(userName, ct);

        var dataListDto = dataListEntity
            .Select(mapper.Map);

        return new()
        {
            ResponseBody = dataListDto,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetLastEntitiesAsync(CancellationToken ct = default)
    {
        var lastEntities = await entityRepository.GetLastEntitiesAsync(3, ct);

        var dataListDto = lastEntities
            .Select(mapper.Map);

        return new()
        {
            ResponseBody = dataListDto,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetPolygonAsync(int id, CancellationToken ct = default)
    {
        var entity = await entityRepository.GetByIdAsync(id, ct);

        if (entity?.Polygon != null)
        {
            return new()
            {
                ResponseBody = JsonSerializer.Deserialize<object>(geoJsonWriter.Write(entity.Polygon))!,
            };
        }
        else
        {
            return new(true)
            {
                StatusCode = HttpStatusCode.NotFound,
            };
        }
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> AddAsync(InitiativeDto entityData, CancellationToken ct = default)
    {
        // Validate data
        var validationResult = await entityValidator.ValidateAsync(entityData, options => options.IncludeRuleSets("default", "Create"), ct);

        if (!validationResult.IsValid)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(validationResult.Errors),
            };
        }

        // Validate duplicated entities
        var hasDuplicatedEntities = await entityRepository.AnyByNameAsync(entityData.Name, ct);

        if (hasDuplicatedEntities)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Initiatives.Duplicated),
            };
        }

        // Validate users data
        var leaderCount = entityData.Users
            .Select(u => u.Level.Id == (int)InitiativeUserLevelEnum.Leader)
            .Count();

        if (leaderCount > MaxLeadersByInitiative)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Initiatives.LeadersPerInitiative),
            };
        }

        // Validate locations data
        var locationsIds = entityData.Locations
            .Select(l => l.LocationId ?? 0)
            .Distinct()
            .ToArray();

        var initiativeLocationQuery = locationRepository
            .GetQueryable()
            .Where(l => locationsIds.Contains(l.Id));

        var locationsDb = await locationRepository.QueryToListAsync(initiativeLocationQuery, ct);

        if (locationsIds.Length != locationsDb.Count)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Initiatives.InvalidLocationsData),
            };
        }

        var duplicatedLocations = entityData.Locations.Count() > entityData.Locations
            .DistinctBy(l => (l.LocationId, l.Locality))
            .Count();

        if (duplicatedLocations)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Initiatives.DuplicatedLocationsData),
            };
        }

        var hasDepartmentsWithLocalities = entityData.Locations
            .Join(locationsDb, il => il.LocationId, l => l.Id, (il, l) => new { il, l })
            .Where(lil => !string.IsNullOrWhiteSpace(lil.il.Locality) && lil.l.ParentId == null)
            .Any();

        if (hasDepartmentsWithLocalities)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.InitiativeLocations.LocalityOnlyForMunicipality),
            };
        }

        // Validate users in external system
        var results = new Dictionary<string, bool>();
        var userTasks = entityData.Users.Select(async user =>
        {
            results[user.UserName] = await iamService.UserExistsAsync(user.UserName, ct);
        });

        await Task.WhenAll(userTasks);

        var invalidUsers = results.Any(r => !r.Value);

        if (invalidUsers)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Initiatives.InvalidUsers),
            };
        }

        // Build entity data
        var entity = mapper.Map(entityData);
        entity.CreationDate = DateTime.Now;
        entity.Coordinate = await entityRepository.GetCentroidAsync(locationsIds, ct);

        // Calculate polygon area
        entity.PolygonArea = await CalculatePolygonAreaAsync(entity, ct);

        // Save data
        entity = await entityRepository.AddAsync(entity, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Create, "Added initiative", "{@EntityData}", entityData);

        return new()
        {
            ResponseBody = entityData,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> UpdateAsync(int id, string userName, bool userIsAdmin, InitiativeDto entityData, CancellationToken ct = default)
    {
        // Validate user permissions
        if (!await entityRepository.AuthorizedEntityModifyAsync(id, userName, userIsAdmin, ct))
        {
            return new(true)
            {
                StatusCode = HttpStatusCode.Forbidden,
            };
        }

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

        if (!entity.Enabled)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.ElementDisabled),
            };
        }

        // Validate duplicated entities
        var hasDuplicatedEntities = await entityRepository.IsDuplicatedAsync(id, entityData.Name, ct);

        if (hasDuplicatedEntities)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Initiatives.Duplicated),
            };
        }

        // Update entity data
        mapper.Update(entity, entityData);

        // Recalculate polygon area if locations might have changed
        entity.PolygonArea = await CalculatePolygonAreaAsync(entity, ct);

        await entityRepository.UpdateAsync(entity, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Update, "Updated initiative", "{@EntityData}", entityData);

        return new()
        {
            ResponseBody = entityData,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> UploadImageAsync(int id, IInputFile formFile, InitiativeImageType imageType, CancellationToken ct = default)
    {
        // Validate entity
        var entity = await entityRepository.GetByIdAsync(id, ct);

        if (entity == null)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.ElementNotFound),
            };
        }

        if (!entity.Enabled)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.ElementDisabled),
            };
        }

        // Validate image
        if (formFile.IsEmpty())
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Files.Empty),
            };
        }

        if (!formFile.IsValidImage())
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Files.InvalidFormat),
            };
        }

        // Compress and convert image
        using var originalImageStream = formFile.OpenStream();
        var compressedImageStream = await imageUtilsService.CompressToWebpAsync(originalImageStream, 75, ct);

        if (compressedImageStream == null)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Files.ProcessingError),
            };
        }

        // Upload/Overwrite image
        var imageTypeStr = imageType.ToString("G").ToLower(CultureInfo.CurrentCulture);
        var fileName = $"{StoragePrefix}/{id}/{imageTypeStr}/{FileUtils.ComputeFileHash(compressedImageStream)}.webp";
        var fileUri = new Uri($"{storageService.BaseUrl}/{fileName}");
        var uploadSuccessful = await storageService.UploadFileAsync(fileName, compressedImageStream, MediaTypeNames.Image.ImageWebp, ct);

        if (uploadSuccessful)
        {
            Uri oldImageUrl = null;
            switch (imageType)
            {
                case InitiativeImageType.Image:
                    oldImageUrl = entity.ImageUrl;
                    entity.ImageUrl = fileUri;
                    break;
                case InitiativeImageType.Banner:
                    oldImageUrl = entity.BannerUrl;
                    entity.BannerUrl = fileUri;
                    break;
                default:
                    return new(true)
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Initiatives.InvalidImageType, data: $"{imageType:G}"),
                    };
            }

            if (oldImageUrl != null && oldImageUrl != fileUri)
            {
                await storageService.DeleteFileAsync(oldImageUrl.ToString(), ct);
            }

            await entityRepository.UpdateAsync(entity, ct);

            var entityData = mapper.Map(entity);

            logger.AddLog(LogType.Update, "Updated initiative image", $"(type: {imageTypeStr}): {{@EntityData}}", entityData);

            return new()
            {
                ResponseBody = entityData,
            };
        }

        return new(true)
        {
            StatusCode = HttpStatusCode.InternalServerError,
            ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Files.Storage),
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> RemoveImageAsync(int id, InitiativeImageType imageType, CancellationToken ct = default)
    {
        // Validate entity
        var entity = await entityRepository.GetByIdAsync(id, ct);

        if (entity == null)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.ElementNotFound),
            };
        }

        if (!entity.Enabled)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.ElementDisabled),
            };
        }

        // Validate image
        Uri fileName;
        switch (imageType)
        {
            case InitiativeImageType.Image:
                fileName = entity.ImageUrl;
                entity.ImageUrl = null;
                break;
            case InitiativeImageType.Banner:
                fileName = entity.BannerUrl;
                entity.BannerUrl = null;
                break;
            default:
                return new(true)
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Initiatives.InvalidImageType, data: $"{imageType:G}"),
                };
        }

        if (fileName == null)
        {
            return new();
        }

        // Remove image
        var processSuccessful = await storageService.DeleteFileAsync(fileName.ToString(), ct);

        if (processSuccessful)
        {
            await entityRepository.UpdateAsync(entity, ct);

            var imageTypeStr = imageType.ToString("G").ToLower(CultureInfo.CurrentCulture);
            var entityData = mapper.Map(entity);

            logger.AddLog(LogType.Update, "Deleted initiative image", $"(type: {imageTypeStr}): {{@EntityData}}", entityData);

            return new();
        }

        return new(true)
        {
            StatusCode = HttpStatusCode.InternalServerError,
            ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Files.Storage),
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> UpdatePolygonAsync(int id, string geoJsonString, CancellationToken ct = default)
    {
        // Validate entity
        var entity = await entityRepository.GetByIdAsync(id, ct);

        if (entity == null)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.ElementNotFound),
            };
        }

        if (!entity.Enabled)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.ElementDisabled),
            };
        }

        Geometry geometry;

        try
        {
            // Read the GeoJSON and convert it to geometry
            geometry = geoJsonReader.Read<Geometry>(geoJsonString);
        }
        catch (JsonException)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Initiatives.InvalidJson),
            };
        }

        // Validate polygon
        if (geometry is not Polygon polygon)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Initiatives.InvalidGeojson),
            };
        }

        // Check SRID
        polygon.SRID = 4326;

        // Update entity
        entity.Polygon = polygon;
        entity.Coordinate = polygon.Centroid;
        entity.PolygonArea = await CalculatePolygonAreaAsync(entity, ct);

        await entityRepository.UpdateAsync(entity, ct);

        var entityData = mapper.Map(entity);

        logger.AddLog(LogType.Update, $"Updated initiative polygon", "{{@EntityData}}", entityData);

        return new()
        {
            ResponseBody = entityData,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> EnableAsync(int id, CancellationToken ct = default) => await DisableOrEnableAsync(id, false, ct);

    /// <inheritdoc/>
    public async Task<CustomWebResponse> DisableAsync(int id, CancellationToken ct = default) => await DisableOrEnableAsync(id, true, ct);

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetByLocationAsync(int? locationId = null, CancellationToken ct = default)
    {
        var initiatives = await entityRepository.GetActiveInitiativesWithCoordinatesByLocationAsync(locationId, ct);
        var result = initiatives.Select(i => new InitiativeGeoData
        {
            InitiativeId = i.InitiativeId,
            InitiativeName = i.InitiativeName,
            Coordinate = i.Coordinate,
        }).ToList();

        return new CustomWebResponse
        {
            ResponseBody = result,
        };
    }

    /// <summary>
    /// Calculate polygon area for an initiative.
    /// </summary>
    /// <param name="entity">Initiative entity.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Area in square kilometers.</returns>
    private async Task<double> CalculatePolygonAreaAsync(Initiative entity, CancellationToken ct = default)
    {
        // If initiative has a polygon, calculate its area
        if (entity.Polygon != null && !entity.Polygon.IsEmpty)
        {
            return GeometryUtils.CalculatePolygonAreaInSquareKilometers(entity.Polygon as Polygon);
        }

        // If no polygon, calculate area from locations
        if (entity.InitiativeLocations != null && entity.InitiativeLocations.Count > 0)
        {
            var locationIds = entity.InitiativeLocations
                .Where(il => il.LocationId > 0)
                .Select(il => il.LocationId)
                .ToList();

            if (locationIds.Count > 0)
            {
                return await locationService.CalculateTotalAreaForLocationsAsync(locationIds, ct);
            }
        }

        return 0;
    }

    /// <summary>
    /// Disable or enable element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="disable">Disable flag.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    private async Task<CustomWebResponse> DisableOrEnableAsync(int id, bool disable, CancellationToken ct = default)
    {
        var entity = await entityRepository.GetByIdAsync(id, ct);

        if (entity == null)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.ElementNotFound),
            };
        }

        entity.Enabled = !disable;
        await entityRepository.UpdateAsync(entity, ct);

        var entityData = mapper.Map(entity);

        logger.AddLog(LogType.Update, $"{(disable ? "Disabled" : "Enabled")} initiative", "{@EntityData}", entityData);

        return new()
        {
            ResponseBody = entityData,
        };
    }
}
