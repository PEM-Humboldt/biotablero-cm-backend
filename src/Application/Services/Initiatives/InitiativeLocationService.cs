namespace IAVH.BioTablero.CM.Application.Services.Initiatives;

using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Initiatives;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Locations;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.GeoEnums;
using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

/// <summary>
/// Initiative Location service.
/// </summary>
public class InitiativeLocationService : ServiceRead<InitiativeLocation, InitiativeLocationDto, int>, IInitiativeLocationService
{
    private new readonly IInitiativeLocationRepository entityRepository;
    private readonly IValidator<InitiativeLocationDto> entityValidator;
    private readonly ILogger logger;
    private new readonly IMapperCreateReadAndUpdate<InitiativeLocation, InitiativeLocationDto> mapper;
    private readonly ILocationRepository locationRepository;
    private readonly IInitiativeRepository initiativeRepository;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="errorTranslator">Error translator.</param>
    /// <param name="entityValidator">Entity validator.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="locationRepository">Location repository.</param>
    /// <param name="initiativeRepository">Initiative repository.</param>
    public InitiativeLocationService(
        IInitiativeLocationRepository entityRepository,
        IMapperCreateReadAndUpdate<InitiativeLocation, InitiativeLocationDto> mapper,
        IValidationErrorTranslator errorTranslator,
        IValidator<InitiativeLocationDto> entityValidator,
        ILogger logger,
        ILocationRepository locationRepository,
        IInitiativeRepository initiativeRepository)
        : base(entityRepository, mapper, errorTranslator)
    {
        this.entityRepository = entityRepository;
        this.mapper = mapper;
        this.entityValidator = entityValidator;
        this.logger = logger;
        this.locationRepository = locationRepository;
        this.initiativeRepository = initiativeRepository;
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
    public async Task<CustomWebResponse> AddAsync(string userName, bool userIsAdmin, InitiativeLocationDto entityData, CancellationToken ct = default)
    {
        // Validate user permissions
        var initiativeId = entityData.InitiativeId ?? 0;

        if (!await initiativeRepository.AuthorizedEntityModifyAsync(initiativeId, userName, userIsAdmin, ct))
        {
            return new(true)
            {
                StatusCode = HttpStatusCode.Forbidden,
            };
        }

        // Validate data
        var validationResult = await entityValidator.ValidateAsync(entityData, options => options.IncludeRuleSets("default", "Create"), ct);

        if (!validationResult.IsValid)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(validationResult.Errors),
            };
        }

        // Validate initiative
        var initiative = await initiativeRepository.GetByIdAsync(initiativeId, ct);

        if (initiative == null)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Initiatives.NotFound),
            };
        }

        if (!initiative.Enabled)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Initiatives.Disabled),
            };
        }

        // Validate location
        var locationId = entityData.LocationId ?? 0;
        var location = await locationRepository.GetByIdAsync(locationId, ct);

        if (location == null)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Locations.NotFound),
            };
        }

        if (!string.IsNullOrWhiteSpace(entityData.Locality) && location.Level != (byte)LocationLevel.Municipality)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.InitiativeLocations.LocalityOnlyForMunicipality),
            };
        }

        // Validate duplicated entities
        var capitalizedLocalityName = entityData.Locality?.Capitalize();
        var hasDuplicatedEntities = await entityRepository.IsDuplicatedAsync(initiativeId, locationId, capitalizedLocalityName, ct);

        if (hasDuplicatedEntities)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.InitiativeLocations.Duplicated),
            };
        }

        // Build entity data
        var entity = mapper.Map(entityData);
        entity.Locality = capitalizedLocalityName;

        // Save data
        entity = await entityRepository.AddAsync(entity, ct);

        // Update initiative data
        initiative.Coordinate = await initiativeRepository.GetCentroidAsync(initiativeId, ct);
        initiative.MainLocationId = await locationRepository.GetDepartmentIdByCoordinateAsync(initiative.Coordinate, ct);
        await initiativeRepository.UpdateAsync(initiative, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Create, "Added initiative location", "{@EntityData}", entityData);

        return new()
        {
            ResponseBody = entityData,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> UpdateAsync(int id, string userName, bool userIsAdmin, InitiativeLocationDto entityData, CancellationToken ct = default)
    {
        // Validate user permissions
        var entity = await entityRepository.GetByIdAsync(id, ct);
        var initiativeId = entity?.InitiativeId ?? 0;

        if (!await initiativeRepository.AuthorizedEntityModifyAsync(initiativeId, userName, userIsAdmin, ct))
        {
            return new(true)
            {
                StatusCode = HttpStatusCode.Forbidden,
            };
        }

        // Validate entity
        if (entity == null)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.ElementNotFound),
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

        // Validate initiative
        var initiative = await initiativeRepository.GetByIdAsync(initiativeId, ct);

        if (!initiative.Enabled)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Initiatives.Disabled),
            };
        }

        // Validate location
        var locationId = entityData.LocationId ?? 0;
        var location = await locationRepository.GetByIdAsync(locationId, ct);

        if (location == null)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Locations.NotFound),
            };
        }

        if (!string.IsNullOrWhiteSpace(entityData.Locality) && location.Level != (byte)LocationLevel.Municipality)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.InitiativeLocations.LocalityOnlyForMunicipality),
            };
        }

        // Validate duplicated entities
        entityData.Locality = entityData.Locality?.Capitalize();
        var hasDuplicatedEntities = await entityRepository.IsDuplicatedAsync(id, entity.InitiativeId, locationId, entityData.Locality, ct);

        if (hasDuplicatedEntities)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.InitiativeLocations.Duplicated),
            };
        }

        // Update entity data
        mapper.Update(entity, entityData);

        // Update initiative data
        initiative.Coordinate = await initiativeRepository.GetCentroidAsync(initiativeId, ct);
        initiative.MainLocationId = await locationRepository.GetDepartmentIdByCoordinateAsync(initiative.Coordinate, ct);
        await initiativeRepository.UpdateAsync(initiative, ct);

        await entityRepository.UpdateAsync(entity, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Update, "Updated initiative location", "{@EntityData}", entityData);

        return new()
        {
            ResponseBody = entityData,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> DeleteAsync(int id, string userName, bool userIsAdmin, CancellationToken ct = default)
    {
        var entity = await entityRepository.GetByIdAsync(id, ct);
        var initiativeId = entity?.InitiativeId ?? 0;

        // Validate user permissions
        if (!await initiativeRepository.AuthorizedEntityModifyAsync(initiativeId, userName, userIsAdmin, ct))
        {
            return new(true)
            {
                StatusCode = HttpStatusCode.Forbidden,
            };
        }

        // Validate entity
        if (entity == null)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.ElementNotFound),
            };
        }

        // Validate initiative
        var initiative = await initiativeRepository.GetByIdAsync(initiativeId, ct);

        if (!initiative.Enabled)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Initiatives.Disabled),
            };
        }

        // Validate number of locations
        var totalLocations = await entityRepository.CountByInitiativeAsync(entity.InitiativeId, ct);

        if (totalLocations is <= 1)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.InitiativeLocations.LocationsRequired),
            };
        }

        await entityRepository.DeleteAsync(entity, ct);

        // Update initiative data
        initiative.Coordinate = await initiativeRepository.GetCentroidAsync(initiativeId, ct);
        initiative.MainLocationId = await locationRepository.GetDepartmentIdByCoordinateAsync(initiative.Coordinate, ct);
        await initiativeRepository.UpdateAsync(initiative, ct);

        var entityData = mapper.Map(entity);

        logger.AddLog(LogType.Delete, "Deleted initiative location", "{@EntityData}", entityData);

        return new();
    }
}
