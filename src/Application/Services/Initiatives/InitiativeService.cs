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

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Specifications;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;
using IAVH.BioTablero.CM.Core.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using Microsoft.AspNetCore.OData.Query;

using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;
using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

using InitiativeUserLevelEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.InitiativeUserLevel;
using LocationEntity = IAVH.BioTablero.CM.Core.Domain.Entities.Geo.Location;

/// <summary>
/// Initiative service.
/// </summary>
public class InitiativeService : ServiceRead<Initiative, InitiativeDto, int, InitiativeSpec>, IInitiativeService
{
    private const int MaxLeadersByInitiative = 3;
    private const string StoragePrefix = "initiatives";
    private readonly IValidator<InitiativeDto> entityValidator;
    private readonly ILogger logger;
    private new readonly IInitiativeRepository entityRepository;
    private readonly IRepository<LocationEntity> locationRepository;
    private readonly IIamService iamService;
    private readonly IStorageService storageService;
    private readonly ILocationService locationService;
    private readonly GeoJsonWriter geoJsonWriter;
    private readonly GeoJsonReader geoJsonReader;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="entityValidator">Entity validator.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="locationRepository">Initiative Location repository.</param>
    /// <param name="storageService">Storage service.</param>
    /// <param name="iamService">IAM service.</param>
    /// <param name="locationService">Location service.</param>
    public InitiativeService(
        IInitiativeRepository entityRepository,
        IMapper<Initiative, InitiativeDto> mapper,
        IValidator<InitiativeDto> entityValidator,
        ILogger logger,
        IRepository<LocationEntity> locationRepository,
        IStorageService storageService,
        IIamService iamService,
        ILocationService locationService)
        : base(entityRepository, mapper)
    {
        this.entityRepository = entityRepository;
        this.entityValidator = entityValidator;
        this.logger = logger;
        this.locationRepository = locationRepository;
        this.storageService = storageService;
        this.iamService = iamService;
        this.locationService = locationService;

        geoJsonWriter = new GeoJsonWriter();
        geoJsonReader = new GeoJsonReader();
    }

    /// <summary>
    /// Get elements list (OData).
    /// </summary>
    /// <param name="queryOptions">OData query options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public override async Task<CustomWebResponse> GetListAsync(ODataQueryOptions<Initiative> queryOptions, CancellationToken ct = default)
    {
        var query = entityRepository.GetQueryable();
        query = entityRepository.IncludeOdataEntities(query);

        return await GetOdataListByQueryAsync(query, queryOptions, ct);
    }

    /// <summary>
    /// Get entity polygon.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
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

    /// <summary>
    /// Add element.
    /// </summary>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> AddAsync(InitiativeDto entityData, CancellationToken ct = default)
    {
        // Validate data
        var validationResult = await entityValidator.ValidateAsync(entityData, options => options.IncludeRuleSets("default", "Create"), ct);

        if (!validationResult.IsValid)
        {
            return new CustomWebResponse(true)
            {
                Message = "Validation errors",
                ResponseBody = validationResult.Errors
                    .Select(error => error.ErrorMessage),
            };
        }

        // Validate duplicated entities
        var hasDuplicatedEntities = await entityRepository.AnyAsync(new InitiativeSpec(entityData.Name), ct);

        if (hasDuplicatedEntities)
        {
            return new CustomWebResponse(true)
            {
                Message = "There is already an initiative with the same name",
            };
        }

        // Validate users data
        var leaderCount = entityData.Users
            .Select(u => u.Level.Id == (int)InitiativeUserLevelEnum.Leader)
            .Count();

        if (leaderCount > MaxLeadersByInitiative)
        {
            return new CustomWebResponse(true)
            {
                Message = $"The number of leaders per initiative should be between 1 and 3",
            };
        }

        // Validate locations data
        var locationsIds = entityData.Locations
            .Select(l => l.LocationId ?? 0)
            .ToArray();

        var initiativeLocationQuery = locationRepository
            .GetQueryable()
            .Where(l => locationsIds.Contains(l.Id));

        var locationsDb = await locationRepository.QueryToListAsync(initiativeLocationQuery, ct);

        if (locationsIds.Length != locationsDb.Count)
        {
            return new CustomWebResponse(true)
            {
                Message = $"Invalid initiative locations data",
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
            return new CustomWebResponse(true)
            {
                Message = $"One or more users are invalid or do not exist",
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

        logger.AddLog(LogType.Create, "Added initiative: {@EntityData}", entityData);

        return new CustomWebResponse()
        {
            ResponseBody = entityData,
        };
    }

    /// <summary>
    /// Update element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> UpdateAsync(int id, InitiativeDto entityData, CancellationToken ct = default)
    {
        // Validate data
        var validationResult = await entityValidator.ValidateAsync(entityData, ct);

        if (!validationResult.IsValid)
        {
            return new CustomWebResponse(true)
            {
                Message = "Validation errors",
                ResponseBody = validationResult.Errors
                    .Select(error => error.ErrorMessage),
            };
        }

        // Validate duplicated entities
        var hasDuplicatedEntities = await entityRepository.AnyAsync(InitiativeSpec.GetDuplicatesSpec(id, entityData.Name), ct);

        if (hasDuplicatedEntities)
        {
            return new CustomWebResponse(true)
            {
                Message = "There is already an initiative with the same name",
            };
        }

        // Validate entity
        var entity = await entityRepository.GetByIdAsync(id, ct);

        if (entity == null)
        {
            return new CustomWebResponse(true)
            {
                Message = MessageConstants.NotFound,
            };
        }

        // Update entity data
        entity.Name = entityData.Name;
        entity.Description = entityData.Description;

        // Recalculate polygon area if locations might have changed
        entity.PolygonArea = await CalculatePolygonAreaAsync(entity, ct);

        await entityRepository.UpdateAsync(entity, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Update, "Updated initiative: {@EntityData}", entityData);

        return new CustomWebResponse()
        {
            ResponseBody = entityData,
        };
    }

    /// <summary>
    /// Upload image.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="formFile">Image data.</param>
    /// <param name="imageType">Initiative image type.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> UploadImageAsync(int id, IInputFile formFile, InitiativeImageType imageType, CancellationToken ct)
    {
        if (formFile.IsEmpty())
        {
            return new CustomWebResponse(true)
            {
                Message = "The file is empty",
            };
        }

        if (!formFile.IsValidImage())
        {
            return new CustomWebResponse(true)
            {
                Message = "Invalid file format",
            };
        }

        // Validate entity
        var entity = await entityRepository.GetByIdAsync(id, ct);

        if (entity == null)
        {
            return new CustomWebResponse(true)
            {
                Message = MessageConstants.NotFound,
            };
        }

        // Upload/Overwrite image
        var imageTypeStr = imageType.ToString("G").ToLower(CultureInfo.CurrentCulture);
        var fileName = $"{StoragePrefix}/{id}/{imageTypeStr}";
        var fileUri = new Uri($"{storageService.BaseUrl}/{fileName}");
        var uploadSuccessful = await storageService.UploadFileAsync(fileName, formFile, ct);

        if (uploadSuccessful)
        {
            switch (imageType)
            {
                case InitiativeImageType.Image:
                    entity.ImageUrl = fileUri;
                    break;
                case InitiativeImageType.Banner:
                    entity.BannerUrl = fileUri;
                    break;
                default:
                    return new CustomWebResponse(true)
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        Message = $"Invalid image type: {imageType:G}",
                    };
            }

            await entityRepository.UpdateAsync(entity, ct);

            var entityData = mapper.Map(entity);

            logger.AddLog(LogType.Update, $"Updated initiative image (type: {imageTypeStr}): {{@EntityData}}", entityData);

            return new CustomWebResponse()
            {
                ResponseBody = entityData,
            };
        }

        return new CustomWebResponse(true)
        {
            StatusCode = HttpStatusCode.InternalServerError,
            Message = "Storage server error",
        };
    }

    /// <summary>
    /// Update entity polygon.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="geoJsonString">Polygon data (string).</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> UpdatePolygonAsync(int id, string geoJsonString, CancellationToken ct = default)
    {
        // Validate entity
        var entity = await entityRepository.GetByIdAsync(id, ct);

        if (entity == null)
        {
            return new CustomWebResponse(true)
            {
                Message = MessageConstants.NotFound,
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
            return new CustomWebResponse(true)
            {
                Message = "Invalid JSON object",
            };
        }

        // Validate polygon
        if (geometry is not Polygon polygon)
        {
            return new CustomWebResponse(true)
            {
                Message = "GeoJSON type should be 'Polygon'",
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

        logger.AddLog(LogType.Update, $"Updated initiative polygon: {{@EntityData}}", entityData);

        return new CustomWebResponse()
        {
            ResponseBody = entityData,
        };
    }

    /// <summary>
    /// Enable element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> EnableAsync(int id, CancellationToken ct = default) => await DisableOrEnableAsync(id, false, ct);

    /// <summary>
    /// Disable element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> DisableAsync(int id, CancellationToken ct = default) => await DisableOrEnableAsync(id, true, ct);

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
            return new CustomWebResponse(true)
            {
                Message = MessageConstants.NotFound,
            };
        }

        entity.Enabled = !disable;
        await entityRepository.UpdateAsync(entity, ct);

        var entityData = mapper.Map(entity);

        logger.AddLog(LogType.Update, string.Concat($"{(disable ? "Disabled" : "Enabled")}", "initiative: {@EntityData}"), entityData);

        return new CustomWebResponse()
        {
            ResponseBody = entityData,
        };
    }
}
