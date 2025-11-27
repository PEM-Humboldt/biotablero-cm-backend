namespace IAVH.BioTablero.CM.Application.Services.TerritoryStory;

using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;
using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.Services.TerritoryStory;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.TerritoryStories;

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
    private readonly ITerritoryStoryVideoRepository territoryStoryVideoRepository;
    private readonly IVideoHelperService videoHelperService;
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
    /// <param name="territoryStoryVideoRepository">Territory Story Video repository.</param>
    /// <param name="videoHelperService">Video Helper service.</param>
    /// <param name="initiativeUserRepository">Initiative User repository.</param>
    public TerritoryStoryService(
        ITerritoryStoryRepository entityRepository,
        IMapper<TerritoryStory, TerritoryStoryDto> mapper,
        IValidator<TerritoryStoryDto> entityValidator,
        ILogger logger,
        IInitiativeRepository initiativeRepository,
        ITerritoryStoryLikeRepository territoryStoryLikeRepository,
        ITerritoryStoryVideoRepository territoryStoryVideoRepository,
        IVideoHelperService videoHelperService,
        IInitiativeUserRepository initiativeUserRepository)
        : base(entityRepository, mapper)
    {
        this.entityRepository = entityRepository;
        this.entityValidator = entityValidator;
        this.logger = logger;
        this.initiativeRepository = initiativeRepository;
        this.territoryStoryLikeRepository = territoryStoryLikeRepository;
        this.territoryStoryVideoRepository = territoryStoryVideoRepository;
        this.videoHelperService = videoHelperService;
        this.initiativeUserRepository = initiativeUserRepository;
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetItemAsync(int id, string userName, CancellationToken ct = default)
    {
        // Validate user level and permissions
        var entityExists = await entityRepository.AnyAsync(id, ct);

        if (entityExists)
        {
            var authorizedUserAction = await entityRepository.AuthorizedEntityReadAsync(id, userName, ct);

            if (!authorizedUserAction)
            {
                return new CustomWebResponse(true)
                {
                    StatusCode = HttpStatusCode.Forbidden,
                };
            }
        }

        return await GetItemAsync(id, ct);
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetByInitiativeAsync(int initiativeId, string userName, CancellationToken ct = default)
    {
        var dataListEntity = await entityRepository.GetByInitiativeAndUserNameAsync(initiativeId, userName, ct);

        var dataListDto = dataListEntity
            .Select(mapper.Map);

        return new()
        {
            ResponseBody = dataListDto,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> AddAsync(TerritoryStoryDto entityData, CancellationToken ct = default)
    {
        // Validate user level and permissions
        var authorizedUserAction = await entityRepository.AuthorizedEntityModifyAsync(null, entityData.AuthorUserName, ct);

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

        if (entityData.Videos.Any())
        {
            // Validate duplicated videos
            var videosUrls = entityData.Videos
                .Select(e => new Uri(e.FileUrl))
                .ToArray();

            var hasDuplicatedEntitiesVideos = await territoryStoryVideoRepository.AnyDuplicatedAsync(videosUrls, ct);

            if (hasDuplicatedEntitiesVideos)
            {
                return new CustomWebResponse(true)
                {
                    Message = "There is already at least one video with the same URL",
                };
            }

            // Validate if the videos exist
            foreach (var videoUrl in videosUrls)
            {
                var videoExists = await videoHelperService.VideoExistsAsync(videoUrl.ToString(), ct);

                if (!videoExists)
                {
                    return new CustomWebResponse(true)
                    {
                        Message = $"The video URL '{videoUrl}' does not exist",
                    };
                }
            }
        }

        // Build entity data
        entityData.Images = null;
        entityData.Enabled = true;
        entityData.FeaturedContent = false;
        var entity = mapper.Map(entityData);
        entity.CreationDate = DateTime.Now;

        // Save data
        entity = await entityRepository.AddAsync(entity, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Create, "Added territory story", "{@EntityData}", entityData);

        return new CustomWebResponse()
        {
            ResponseBody = entityData,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> UpdateAsync(int id, string userName, TerritoryStoryDto entityData, CancellationToken ct = default)
    {
        // Validate user level and permissions
        var authorizedUserAction = await entityRepository.AuthorizedEntityModifyAsync(id, userName, ct);

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

        if (!entity.Enabled)
        {
            return new CustomWebResponse(true)
            {
                Message = MessageConstants.DisabledElement,
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
        entity.Restricted = entityData.Restricted ?? entity.Restricted;

        await entityRepository.UpdateAsync(entity, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Update, "Updated territory story video", "{@EntityData}", entityData);

        return new CustomWebResponse()
        {
            ResponseBody = entityData,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> LikeActionAsync(TerritoryStoryLikeDto entityData, CancellationToken ct = default)
    {
        // Validate Territory Story
        var entity = await entityRepository.GetByIdAsync(entityData.TerritoryStoryId, ct);

        if (entity == null)
        {
            return new CustomWebResponse(true)
            {
                Message = MessageConstants.NotFound,
            };
        }

        if (!entity.Enabled)
        {
            return new CustomWebResponse(true)
            {
                Message = MessageConstants.DisabledElement,
            };
        }

        var hasDuplicatedEntities = await territoryStoryLikeRepository.IsDuplicatedAsync(entityData.TerritoryStoryId, entityData.UserName, ct);

        if (hasDuplicatedEntities)
        {
            var like = await territoryStoryLikeRepository.GetByTerritoryStoryAndUserNameAsync(entityData.TerritoryStoryId, entityData.UserName, ct);
            await territoryStoryLikeRepository.DeleteAsync(like, ct);
            logger.AddLog(LogType.Delete, $"Unliked territory story", "{@EntityData}", entityData);
        }
        else
        {
            var like = new TerritoryStoryLike()
            {
                CreationDate = DateTime.Now,
                TerritoryStoryId = entityData.TerritoryStoryId,
                UserName = entityData.UserName,
            };

            await territoryStoryLikeRepository.AddAsync(like, ct);
            logger.AddLog(LogType.Create, $"Liked territory story", "{@EntityData}", entityData);
        }

        return new CustomWebResponse();
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> FeaturedContentActionAsync(int id, string userName, CancellationToken ct = default)
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

        if (!entity.Enabled)
        {
            return new CustomWebResponse(true)
            {
                Message = MessageConstants.DisabledElement,
            };
        }

        // Validate user level and permissions
        var userIsLeader = await initiativeUserRepository.AnyByInitiativeUserAndLevelAsync(entity.InitiativeId, userName, (int)InitiativeUserLevelEnum.Leader, ct);

        if (!userIsLeader)
        {
            return new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.Forbidden,
            };
        }

        // Mark territory story as featured content
        entity = await entityRepository.MarkAsFeaturedContentAsync(id, ct);

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

    /// <inheritdoc/>
    public async Task<CustomWebResponse> EnableAsync(int id, string userName, CancellationToken ct = default) => await DisableOrEnableAsync(id, userName, false, ct);

    /// <inheritdoc/>
    public async Task<CustomWebResponse> DisableAsync(int id, string userName, CancellationToken ct = default) => await DisableOrEnableAsync(id, userName, true, ct);

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
        var authorizedUserAction = await entityRepository.AuthorizedEntityModifyAsync(id, userName, ct);

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
