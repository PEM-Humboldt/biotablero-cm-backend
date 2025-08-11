namespace IAVH.BioTablero.CM.Application.Services.Initiatives;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Specifications;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

/// <summary>
/// Initiative Location service.
/// </summary>
public class InitiativeLocationService : ServiceRead<InitiativeLocation, InitiativeLocationDto, int, InitiativeLocationSpec>, IInitiativeLocationService
{
    private readonly IValidator<InitiativeLocationDto> entityValidator;
    private readonly ILogger logger;
    private readonly IRepository<Location> locationRepository;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="entityValidator">Entity validator.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="locationRepository">Location repository.</param>
    public InitiativeLocationService(
        IRepository<InitiativeLocation> entityRepository,
        IMapper<InitiativeLocation, InitiativeLocationDto> mapper,
        IValidator<InitiativeLocationDto> entityValidator,
        ILogger logger,
        IRepository<Location> locationRepository)
        : base(entityRepository, mapper)
    {
        this.entityValidator = entityValidator;
        this.logger = logger;
        this.locationRepository = locationRepository;
    }

    /// <summary>
    /// Get entities by initiative.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> GetByInitiative(int initiativeId, CancellationToken ct = default)
    {
        var dataListEntity = await entityRepository.ListAsync(InitiativeLocationSpec.InitiativeIdSpec(initiativeId), ct);

        var dataListDto = dataListEntity
            .Select(mapper.Map);

        return new()
        {
            ResponseBody = dataListDto,
        };
    }

    /// <summary>
    /// Add element.
    /// </summary>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> Add(InitiativeLocationDto entityData, CancellationToken ct = default)
    {
        // Validate data
        var validationResult = await entityValidator.ValidateAsync(entityData, options => options.IncludeRuleSets("Create"), ct);

        if (!validationResult.IsValid)
        {
            return new CustomWebResponse(true)
            {
                Message = "Validation errors",
                ResponseBody = validationResult.Errors
                    .Select(error => error.ErrorMessage),
            };
        }

        // Validate initiative
        var initiativeId = entityData.InitiativeId ?? 0;
        var initiativeExists = await entityRepository.AnyAsync(new InitiativeLocationSpec(initiativeId), ct);

        if (!initiativeExists)
        {
            return new CustomWebResponse(true)
            {
                Message = "Initiative not found",
            };
        }

        // Validate location
        var locationId = entityData.LocationId ?? 0;
        var locationExists = await locationRepository.AnyAsync(new LocationSpec(locationId), ct);

        if (!locationExists)
        {
            return new CustomWebResponse(true)
            {
                Message = "Location not found",
            };
        }

        // Validate duplicated entities
        var hasDuplicatedEntities = await entityRepository.AnyAsync(InitiativeLocationSpec.LocationIdAndLocalitySpec(initiativeId, locationId, entityData.Locality), ct);

        if (hasDuplicatedEntities)
        {
            return new CustomWebResponse(true)
            {
                Message = "The location already belongs to the initiative",
            };
        }

        // Build entity data
        var entity = mapper.Map(entityData);
        entity.Locality = entityData.Locality.Capitalize();

        // Save data
        entity = await entityRepository.AddAsync(entity, ct);

        entityData = mapper.Map(entity);

        logger
            .ForContext("CustomRecord", true)
            .ForContext("Type", (int)LogType.Create)
            .Information("Added initiative location: {@entityData}", entityData);

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
    public async Task<CustomWebResponse> Update(int id, InitiativeLocationDto entityData, CancellationToken ct = default)
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

        // Validate entity
        var entity = await entityRepository.GetByIdAsync(id, ct);

        if (entity == null)
        {
            return new CustomWebResponse(true)
            {
                Message = "Not found",
            };
        }

        // Validate location
        var locationId = entityData.LocationId ?? 0;
        var locationExists = await locationRepository.AnyAsync(new LocationSpec(locationId), ct);

        if (!locationExists)
        {
            return new CustomWebResponse(true)
            {
                Message = "Location not found",
            };
        }

        // Validate duplicated entities
        var hasDuplicatedEntities = await entityRepository.AnyAsync(InitiativeLocationSpec.LocationIdAndLocalitySpec(id, entity.InitiativeId, locationId, entityData.Locality), ct);

        if (hasDuplicatedEntities)
        {
            return new CustomWebResponse(true)
            {
                Message = "The location already belongs to the initiative",
            };
        }

        // Update entity data
        entity.LocationId = entityData.LocationId ?? 0;
        entity.Locality = entityData.Locality.Capitalize();

        await entityRepository.UpdateAsync(entity, ct);

        entityData = mapper.Map(entity);

        logger
            .ForContext("CustomRecord", true)
            .ForContext("Type", (int)LogType.Update)
            .Information("Updated initiative location: {@entityData}", entityData);

        return new CustomWebResponse()
        {
            ResponseBody = entityData,
        };
    }

    /// <summary>
    /// Delete element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> Delete(int id, CancellationToken ct = default)
    {
        // Validate entity
        var entity = await entityRepository.GetByIdAsync(id, ct);

        if (entity == null)
        {
            return new CustomWebResponse(true)
            {
                Message = "Not found",
            };
        }

        // Validate number of locations
        var totalLocations = await entityRepository.CountAsync(InitiativeLocationSpec.InitiativeIdSpec(entity.InitiativeId), ct);

        if (totalLocations is <= 1)
        {
            return new CustomWebResponse(true)
            {
                Message = $"At least one location is required per initiative",
            };
        }

        await entityRepository.DeleteAsync(entity, ct);

        var entityData = mapper.Map(entity);

        logger
            .ForContext("CustomRecord", true)
            .ForContext("Type", (int)LogType.Delete)
            .Information("Deleted initiative location: {@entityData}", entityData);

        return new CustomWebResponse();
    }
}
