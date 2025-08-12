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
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

using InitiativeUserLevelEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.InitiativeUserLevel;

/// <summary>
/// Initiative User service.
/// </summary>
public class InitiativeUserService : ServiceRead<InitiativeUser, InitiativeUserDto, int, InitiativeUserSpec>, IInitiativeUserService
{
    private const int MaxLeadersByInitiative = 3;
    private readonly IValidator<InitiativeUserDto> entityValidator;
    private readonly ILogger logger;
    private readonly IIamService iamService;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="entityValidator">Entity validator.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="iamService">IAM service.</param>
    public InitiativeUserService(
        IRepository<InitiativeUser> entityRepository,
        IMapper<InitiativeUser, InitiativeUserDto> mapper,
        IValidator<InitiativeUserDto> entityValidator,
        ILogger logger,
        IIamService iamService)
        : base(entityRepository, mapper)
    {
        this.entityValidator = entityValidator;
        this.logger = logger;
        this.iamService = iamService;
    }

    /// <summary>
    /// Get entities by initiative.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> GetByInitiative(int initiativeId, CancellationToken ct = default)
    {
        var dataListEntity = await entityRepository.ListAsync(InitiativeUserSpec.InitiativeIdSpec(initiativeId), ct);

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
    public async Task<CustomWebResponse> Add(InitiativeUserDto entityData, CancellationToken ct = default)
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
        var initiativeExists = await entityRepository.AnyAsync(new InitiativeUserSpec(initiativeId), ct);

        if (!initiativeExists)
        {
            return new CustomWebResponse(true)
            {
                Message = "Initiative not found",
            };
        }

        // Validate duplicated entities
        var hasDuplicatedEntities = await entityRepository.AnyAsync(InitiativeUserSpec.UserNameSpec(initiativeId, entityData.UserName), ct);

        if (hasDuplicatedEntities)
        {
            return new CustomWebResponse(true)
            {
                Message = "The user already belongs to the initiative",
            };
        }

        // Validate number of leaders
        if (entityData.Level.Id == (int)InitiativeUserLevelEnum.Leader)
        {
            var totalUserLeaders = await entityRepository.CountAsync(InitiativeUserSpec.LevelSpec(initiativeId, (int)InitiativeUserLevelEnum.Leader), ct);

            if (totalUserLeaders >= MaxLeadersByInitiative)
            {
                return new CustomWebResponse(true)
                {
                    Message = $"Initiatives cannot have more than {MaxLeadersByInitiative} leaders",
                };
            }
        }

        // Validate user in external system
        var userExists = await iamService.UserExists(entityData.UserName, ct);

        if (!userExists)
        {
            return new CustomWebResponse(true)
            {
                Message = $"User is invalid or do not exist",
            };
        }

        // Build entity data
        var entity = mapper.Map(entityData);

        // Save data
        entity = await entityRepository.AddAsync(entity, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Create, "Added initiative user: {@entityData}", entityData);

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
    public async Task<CustomWebResponse> Update(int id, InitiativeUserDto entityData, CancellationToken ct = default)
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

        // Validate number of leaders
        var totalUserLeaders = await entityRepository.CountAsync(InitiativeUserSpec.LevelSpec(id, entity.InitiativeId, (int)InitiativeUserLevelEnum.Leader), ct);

        if (entity.LevelId == (int)InitiativeUserLevelEnum.Leader && entityData.Level.Id != (int)InitiativeUserLevelEnum.Leader)
        {
            if (totalUserLeaders is < 1)
            {
                return new CustomWebResponse(true)
                {
                    Message = $"At least one leader is required per initiative",
                };
            }
        }

        if (entityData.Level.Id == (int)InitiativeUserLevelEnum.Leader)
        {
            if (totalUserLeaders >= MaxLeadersByInitiative)
            {
                return new CustomWebResponse(true)
                {
                    Message = $"Initiatives cannot have more than {MaxLeadersByInitiative} leaders",
                };
            }
        }

        // Update entity data
        entity.LevelId = entityData.Level.Id;

        await entityRepository.UpdateAsync(entity, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Update, "Updated initiative user: {@entityData}", entityData);

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

        // Validate number of leaders
        if (entity.LevelId == (int)InitiativeUserLevelEnum.Leader)
        {
            var totalUserLeaders = await entityRepository.CountAsync(InitiativeUserSpec.LevelSpec(entity.InitiativeId, (int)InitiativeUserLevelEnum.Leader), ct);

            if (totalUserLeaders is <= 1)
            {
                return new CustomWebResponse(true)
                {
                    Message = $"At least one leader is required per initiative",
                };
            }
        }

        await entityRepository.DeleteAsync(entity, ct);

        var entityData = mapper.Map(entity);

        logger.AddLog(LogType.Delete, "Deleted initiative user: {@entityData}", entityData);

        return new CustomWebResponse();
    }
}
