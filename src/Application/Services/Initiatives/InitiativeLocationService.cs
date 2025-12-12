namespace IAVH.BioTablero.CM.Application.Services.Initiatives;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Initiatives;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Locations;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

/// <summary>
/// Initiative Location service.
/// </summary>
public class InitiativeLocationService : ServiceRead<InitiativeLocation, InitiativeLocationDto, int>, IInitiativeLocationService
{
    private new readonly IInitiativeLocationRepository entityRepository;
    private readonly IValidator<InitiativeLocationDto> entityValidator;
    private readonly ILogger logger;
    private readonly ILocationRepository locationRepository;
    private readonly IInitiativeRepository initiativeRepository;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="entityValidator">Entity validator.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="locationRepository">Location repository.</param>
    /// <param name="initiativeRepository">Initiative repository.</param>
    public InitiativeLocationService(
        IInitiativeLocationRepository entityRepository,
        IMapper<InitiativeLocation, InitiativeLocationDto> mapper,
        IValidator<InitiativeLocationDto> entityValidator,
        ILogger logger,
        ILocationRepository locationRepository,
        IInitiativeRepository initiativeRepository)
        : base(entityRepository, mapper)
    {
        this.entityRepository = entityRepository;
        this.entityValidator = entityValidator;
        this.logger = logger;
        this.locationRepository = locationRepository;
        this.initiativeRepository = initiativeRepository;
    }

    /// <summary>
    /// Get entities by initiative.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
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

    /// <summary>
    /// Add element.
    /// </summary>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> AddAsync(InitiativeLocationDto entityData, CancellationToken ct = default)
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

        // Validate initiative
        var initiativeId = entityData.InitiativeId ?? 0;
        var initiativeExists = await initiativeRepository.AnyAsync(initiativeId, ct);

        if (!initiativeExists)
        {
            return new CustomWebResponse(true)
            {
                Message = "Initiative not found",
            };
        }

        // Validate location
        var locationId = entityData.LocationId ?? 0;
        var locationExists = await locationRepository.AnyAsync(locationId, ct);

        if (!locationExists)
        {
            return new CustomWebResponse(true)
            {
                Message = "Location not found",
            };
        }

        // Validate duplicated entities
        var capitalizedLocalityName = entityData.Locality.Capitalize();
        var hasDuplicatedEntities = await entityRepository.IsDuplicatedAsync(initiativeId, locationId, capitalizedLocalityName, ct);

        if (hasDuplicatedEntities)
        {
            return new CustomWebResponse(true)
            {
                Message = "The location already belongs to the initiative",
            };
        }

        // Build entity data
        var entity = mapper.Map(entityData);
        entity.Locality = capitalizedLocalityName;

        // Save data
        entity = await entityRepository.AddAsync(entity, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Create, "Added initiative location", "{@EntityData}", entityData);

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
    public async Task<CustomWebResponse> UpdateAsync(int id, InitiativeLocationDto entityData, CancellationToken ct = default)
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
                Message = MessageConstants.NotFound,
            };
        }

        // Validate location
        var locationId = entityData.LocationId ?? 0;
        var locationExists = await locationRepository.AnyAsync(locationId, ct);

        if (!locationExists)
        {
            return new CustomWebResponse(true)
            {
                Message = "Location not found",
            };
        }

        // Validate duplicated entities
        var capitalizedLocalityName = entityData.Locality.Capitalize();
        var hasDuplicatedEntities = await entityRepository.IsDuplicatedAsync(id, entity.InitiativeId, locationId, capitalizedLocalityName, ct);

        if (hasDuplicatedEntities)
        {
            return new CustomWebResponse(true)
            {
                Message = "The location already belongs to the initiative",
            };
        }

        // Update entity data
        entity.LocationId = entityData.LocationId ?? 0;
        entity.Locality = capitalizedLocalityName;

        await entityRepository.UpdateAsync(entity, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Update, "Updated initiative location", "{@EntityData}", entityData);

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
    public async Task<CustomWebResponse> DeleteAsync(int id, CancellationToken ct = default)
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

        // Validate number of locations
        var totalLocations = await entityRepository.CountByInitiativeAsync(entity.InitiativeId, ct);

        if (totalLocations is <= 1)
        {
            return new CustomWebResponse(true)
            {
                Message = $"At least one location is required per initiative",
            };
        }

        await entityRepository.DeleteAsync(entity, ct);

        var entityData = mapper.Map(entity);

        logger.AddLog(LogType.Delete, "Deleted initiative location", "{@EntityData}", entityData);

        return new CustomWebResponse();
    }
}
