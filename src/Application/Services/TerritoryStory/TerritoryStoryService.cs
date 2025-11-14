namespace IAVH.BioTablero.CM.Application.Services.TerritoryStory;

using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.Services.TerritoryStory;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.TerritoryStories;

using Microsoft.AspNetCore.OData.Query;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

using InitiativeUserLevelEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.InitiativeUserLevel;

/// <summary>
/// Territory Story service.
/// </summary>
public class TerritoryStoryService : ServiceRead<TerritoryStory, TerritoryStoryDto, int>, ITerritoryStoryService
{
    private new readonly ITerritoryStoryRepository entityRepository;
    private readonly IValidator<TerritoryStoryDto> entityValidator;
    private readonly ILogger logger;
    private readonly IInitiativeRepository initiativeRepository;
    private readonly ITerritoryStoryLikeRepository territoryStoryLikeRepository;
    private readonly IInitiativeUserRepository initiativeUserRepository;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="entityValidator">Entity validator.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="initiativeRepository">Initiative repository.</param>
    /// <param name="territoryStoryLikeRepository">Territory Story Like repository.</param>
    /// <param name="initiativeUserRepository">Initiative User repository.</param>
    public TerritoryStoryService(
        ITerritoryStoryRepository entityRepository,
        IMapper<TerritoryStory, TerritoryStoryDto> mapper,
        IValidator<TerritoryStoryDto> entityValidator,
        ILogger logger,
        IInitiativeRepository initiativeRepository,
        ITerritoryStoryLikeRepository territoryStoryLikeRepository,
        IInitiativeUserRepository initiativeUserRepository)
        : base(entityRepository, mapper)
    {
        this.entityRepository = entityRepository;
        this.entityValidator = entityValidator;
        this.logger = logger;
        this.initiativeRepository = initiativeRepository;
        this.territoryStoryLikeRepository = territoryStoryLikeRepository;
        this.initiativeUserRepository = initiativeUserRepository;
    }

    /// <summary>
    /// Get elements list (OData).
    /// </summary>
    /// <param name="queryOptions">OData query options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public override async Task<CustomWebResponse> GetListAsync(ODataQueryOptions<TerritoryStory> queryOptions, CancellationToken ct = default)
    {
        var query = entityRepository.GetQueryable();
        query = entityRepository.IncludeOdataEntities(query);

        return await GetOdataListByQueryAsync(query, queryOptions, ct);
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
    public async Task<CustomWebResponse> AddAsync(TerritoryStoryDto entityData, CancellationToken ct = default)
    {
        // Validate user level and permissions
        var authorizedUserAction = await entityRepository.AuthorizedUserAction(null, entityData.AuthorUserName, ct);

        if (!authorizedUserAction)
        {
            return new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.Forbidden,
            };
        }

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
        var initiativeExists = await initiativeRepository.AnyAsync(entityData.InitiativeId.Value, ct);

        if (!initiativeExists)
        {
            return new CustomWebResponse(true)
            {
                Message = "Initiative not found",
            };
        }

        // Validate duplicated entities
        var hasDuplicatedEntities = await entityRepository.IsDuplicatedAsync(entityData.Title, ct);

        if (hasDuplicatedEntities)
        {
            return new CustomWebResponse(true)
            {
                Message = "There is already a territory story with the same title",
            };
        }

        // Build entity data
        var entity = mapper.Map(entityData);
        entity.CreationDate = DateTime.Now;
        entity.Enabled = true;

        // Save data
        entity = await entityRepository.AddAsync(entity, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Create, "Added territory story", "{@EntityData}", entityData);

        return new CustomWebResponse()
        {
            ResponseBody = entityData,
        };
    }

    /// <summary>
    /// Update element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> UpdateAsync(int id, string userName, TerritoryStoryDto entityData, CancellationToken ct = default)
    {
        // Validate user level and permissions
        var authorizedUserAction = await entityRepository.AuthorizedUserAction(id, userName, ct);

        if (!authorizedUserAction)
        {
            return new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.Forbidden,
            };
        }

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

        // Validate duplicated entities
        var hasDuplicatedEntities = await entityRepository.IsDuplicatedAsync(id, entity.Title, ct);

        if (hasDuplicatedEntities)
        {
            return new CustomWebResponse(true)
            {
                Message = "There is already a territory story with the same title",
            };
        }

        // Update entity data
        entity.Title = entityData.Title;
        entity.Text = entityData.Text;
        entity.Keywords = entityData.Keywords;

        await entityRepository.UpdateAsync(entity, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Update, "Updated territory story video", "{@EntityData}", entityData);

        return new CustomWebResponse()
        {
            ResponseBody = entityData,
        };
    }

    /// <summary>
    /// Like button action.
    /// </summary>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> LikeActionAsync(TerritoryStoryLikeDto entityData, CancellationToken ct = default)
    {
        // Validate Territory Story
        var territoryStory = await entityRepository.GetByIdAsync(entityData.TerritoryStoryId, ct);

        if (territoryStory == null)
        {
            return new CustomWebResponse(true)
            {
                Message = "Territory Story not found",
            };
        }

        var hasDuplicatedEntities = await territoryStoryLikeRepository.IsDuplicatedAsync(entityData.TerritoryStoryId, entityData.UserName, ct);

        if (hasDuplicatedEntities)
        {
            var entity = await territoryStoryLikeRepository.GetByTerritoryStoryAndUserNameAsync(entityData.TerritoryStoryId, entityData.UserName, ct);
            await territoryStoryLikeRepository.DeleteAsync(entity, ct);
            logger.AddLog(LogType.Delete, $"Unliked territory story", "{@EntityData}", entityData);
        }
        else
        {
            var entity = new TerritoryStoryLike()
            {
                CreationDate = DateTime.Now,
                TerritoryStoryId = entityData.TerritoryStoryId,
                UserName = entityData.UserName,
            };

            await territoryStoryLikeRepository.AddAsync(entity, ct);
            logger.AddLog(LogType.Create, $"Liked territory story", "{@EntityData}", entityData);
        }

        return new CustomWebResponse();
    }

    /// <summary>
    /// Featured content action.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> FeaturedContentActionAsync(int id, string userName, CancellationToken ct = default)
    {
        // Validate Territory Story
        var territoryStory = await entityRepository.GetByIdAsync(id, ct);

        if (territoryStory != null)
        {
            return new CustomWebResponse(true)
            {
                Message = "Territory Story not found",
            };
        }

        // Validate user level and permissions
        var userIsLeader = await initiativeUserRepository.AnyByInitiativeUserAndLevelAsync(territoryStory.InitiativeId, userName, (int)InitiativeUserLevelEnum.Leader, ct);

        if (!userIsLeader)
        {
            return new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.Forbidden,
            };
        }

        // Mark territory story as featured content
        var entity = await entityRepository.MarkAsFeaturedContent(id, ct);

        if (entity == null)
        {
            return new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Message = "Database error",
            };
        }

        var entityData = mapper.Map(entity);

        logger.AddLog(LogType.Update, "Marked territory story as featured content", "{@EntityData}", entityData);

        return new CustomWebResponse()
        {
            ResponseBody = entityData,
        };
    }

    /// <summary>
    /// Enable element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> EnableAsync(int id, string userName, CancellationToken ct = default) => await DisableOrEnableAsync(id, userName, false, ct);

    /// <summary>
    /// Disable element.initiativeId.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> DisableAsync(int id, string userName, CancellationToken ct = default) => await DisableOrEnableAsync(id, userName, false, ct);

    /// <summary>
    /// Disable or enable element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="disable">Disable flag.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    private async Task<CustomWebResponse> DisableOrEnableAsync(int id, string userName, bool disable, CancellationToken ct = default)
    {
        // Validate user level and permissions
        var authorizedUserAction = await entityRepository.AuthorizedUserAction(id, userName, ct);

        if (!authorizedUserAction)
        {
            return new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.Forbidden,
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

        entity.Enabled = !disable;
        await entityRepository.UpdateAsync(entity, ct);

        var entityData = mapper.Map(entity);

        logger.AddLog(LogType.Update, $"{(disable ? "Disabled" : "Enabled")} territory story", "{@EntityData}", entityData);

        return new CustomWebResponse()
        {
            ResponseBody = entityData,
        };
    }
}
