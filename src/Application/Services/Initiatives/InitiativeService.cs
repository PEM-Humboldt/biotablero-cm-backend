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

using Microsoft.AspNetCore.OData.Query;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

using InitiativeUserLevelEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.InitiativeUserLevel;

/// <summary>
/// Initiative service.
/// </summary>
public class InitiativeService : ServiceRead<Initiative, InitiativeDto, int, InitiativeSpec>, IInitiativeService
{
    private const int MaxLeadersByInitiative = 3;
    private readonly IValidator<InitiativeDto> entityValidator;
    private readonly IRepository<Location> locationRepository;
    private readonly ILogger logger;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="entityValidator">Entity validator.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="initiativeUserRepository">Initiative User repository.</param>
    /// <param name="locationRepository">Initiative Location repository.</param>
    public InitiativeService(
        IRepository<Initiative> entityRepository,
        IMapper<Initiative, InitiativeDto> mapper,
        IValidator<InitiativeDto> entityValidator,
        ILogger logger,
        IRepository<Location> locationRepository)
        : base(entityRepository, mapper)
    {
        this.entityValidator = entityValidator;
        this.logger = logger;
        this.locationRepository = locationRepository;
    }

    /// <summary>
    /// Get elements list (OData).
    /// </summary>
    /// <param name="queryOptions">OData query options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public override async Task<CustomWebResponse> GetList(ODataQueryOptions<Initiative> queryOptions, CancellationToken ct = default)
    {
        // Get only enabled entities
        var query = entityRepository.GetQueryable();
        query = query.Where(e => e.Enabled);

        return await GetOdataListByQuery(query, queryOptions, ct);
    }

    /// <summary>
    /// Add element.
    /// </summary>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> Add(InitiativeDto entityData, CancellationToken ct = default)
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
        var leaderCount = entityData.InitiativeUsers
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
        var locationsIds = entityData.InitiativeLocations
            .Select(l => l.Location.Id);

        var initiativeLocationQuery = locationRepository
            .GetQueryable()
            .Where(l => locationsIds.Contains(l.Id));

        var locationsDb = await locationRepository.QueryToListAsync(initiativeLocationQuery, ct);

        if (locationsIds.Count() != locationsDb.Count)
        {
            return new CustomWebResponse(true)
            {
                Message = $"Invalid initiative locations data",
            };
        }

        // TODO: Add external users data validation!

        // Build entity data
        var entity = mapper.Map(entityData);

        // Save data
        entity = await entityRepository.AddAsync(entity, ct);

        entityData = mapper.Map(entity);

        logger
            .ForContext("CustomRecord", true)
            .ForContext("Type", (int)LogType.Create)
            .Information("Added initiative: {@entityData}", entityData);

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
    public async Task<CustomWebResponse> Update(int id, InitiativeDto entityData, CancellationToken ct = default)
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
                Message = "Not found",
            };
        }

        // Update entity data
        entity.Name = entityData.Name;
        entity.Description = entityData.Description;

        await entityRepository.UpdateAsync(entity, ct);

        entityData = mapper.Map(entity);

        logger
            .ForContext("CustomRecord", true)
            .ForContext("Type", (int)LogType.Update)
            .Information("Updated initiative: {@entityData}", entityData);

        return new CustomWebResponse()
        {
            ResponseBody = entityData,
        };
    }

    /// <summary>
    /// Disable or enable element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="disable">Disable flag.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> Disable(int id, bool disable, CancellationToken ct = default)
    {
        var entity = await entityRepository.GetByIdAsync(id, ct);

        if (entity == null)
        {
            return new CustomWebResponse(true)
            {
                Message = "Not found",
            };
        }

        entity.Enabled = !disable;
        await entityRepository.UpdateAsync(entity, ct);

        var entityData = mapper.Map(entity);
        logger
            .ForContext("CustomRecord", true)
            .ForContext("Type", (int)LogType.Update)
            .Information((disable ? "Disabled" : "Enabled") + " initiative: {@entityData}", entityData);

        return new CustomWebResponse()
        {
            ResponseBody = entityData,
        };
    }
}
