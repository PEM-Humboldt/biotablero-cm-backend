namespace IAVH.BioTablero.CM.Application.Services.TerritoryStories;

using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;
using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Interfaces.Services.TerritoryStories;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.TerritoryStories;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.OData;

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
    private new readonly IMapperCreateReadAndUpdate<TerritoryStory, TerritoryStoryDto> mapper;
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
    /// <param name="errorTranslator">Error translator.</param>
    /// <param name="entityValidator">Entity validator.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="initiativeRepository">Initiative repository.</param>
    /// <param name="territoryStoryLikeRepository">Territory Story Like repository.</param>
    /// <param name="territoryStoryVideoRepository">Territory Story Video repository.</param>
    /// <param name="videoHelperService">Video Helper service.</param>
    /// <param name="initiativeUserRepository">Initiative User repository.</param>
    public TerritoryStoryService(
        ITerritoryStoryRepository entityRepository,
        IMapperCreateReadAndUpdate<TerritoryStory, TerritoryStoryDto> mapper,
        IValidationErrorTranslator errorTranslator,
        IValidator<TerritoryStoryDto> entityValidator,
        ILogger logger,
        IInitiativeRepository initiativeRepository,
        ITerritoryStoryLikeRepository territoryStoryLikeRepository,
        ITerritoryStoryVideoRepository territoryStoryVideoRepository,
        IVideoHelperService videoHelperService,
        IInitiativeUserRepository initiativeUserRepository)
        : base(entityRepository, mapper, errorTranslator)
    {
        this.entityRepository = entityRepository;
        this.mapper = mapper;
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
        // Validate user permissions
        var entity = await entityRepository.GetByIdAsync(id, ct);

        if (entity != null)
        {
            var authorizedUserAction = await entityRepository.AuthorizedEntityReadAsync(id, userName, ct);

            if (!authorizedUserAction)
            {
                return new(true)
                {
                    StatusCode = HttpStatusCode.Forbidden,
                };
            }

            if (!string.IsNullOrWhiteSpace(userName))
            {
                entity.ILikedIt = entity.Likes?.Any(e => e.UserName == userName);
            }

            var dataDto = mapper.Map(entity);

            return new()
            {
                ResponseBody = dataDto,
            };
        }

        return new(true)
        {
            StatusCode = HttpStatusCode.NotFound,
            ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.ElementNotFound),
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetByInitiativeAsync(int initiativeId, string userName, ODataQueryOptions<TerritoryStory> queryOptions, CancellationToken ct = default)
    {
        var query = entityRepository.GetQueryable();
        query = await entityRepository.GetQueryWithInitiativeAndUserNameAsync(initiativeId, userName, query, ct);

        try
        {
            var odataResponse = await GetOdataDtoListByQueryAsync(query, queryOptions, ct);

            foreach (var entity in odataResponse.DataList)
            {
                if (!string.IsNullOrWhiteSpace(userName))
                {
                    entity.ILikedIt = entity?.Likes?.Any(e => e.UserName == userName);
                }
            }

            return GetOdataWebResponse(odataResponse, mapper);
        }
        catch (ODataException)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.OdataInvalidFilter),
            };
        }
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> AddAsync(TerritoryStoryDto entityData, CancellationToken ct = default)
    {
        // Validate user permissions
        var initiativeId = entityData?.InitiativeId ?? 0;
        var authorizedLevels = new int[] { (int)InitiativeUserLevelEnum.Leader, (int)InitiativeUserLevelEnum.Member };
        var authorizedUserAction = await initiativeUserRepository.AnyByInitiativeUserAndLevelsAsync(initiativeId, entityData.AuthorUserName, authorizedLevels, ct);

        if (!authorizedUserAction)
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
        var initiativeExists = await initiativeRepository.AnyAsync(entityData.InitiativeId.Value, ct);

        if (!initiativeExists)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Initiatives.NotFound),
            };
        }

        // Validate duplicated entities
        var hasDuplicatedEntities = await entityRepository.IsDuplicatedAsync(entityData.Title, ct);

        if (hasDuplicatedEntities)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.TerritoryStories.Duplicated),
            };
        }

        if (entityData.Videos?.Any() ?? false)
        {
            // Validate duplicated videos
            var videosUrls = entityData.Videos
                .Select(e => new Uri(e.FileUrl))
                .ToArray();

            var hasDuplicatedEntitiesVideos = await territoryStoryVideoRepository.AnyDuplicatedAsync(videosUrls, ct);

            if (hasDuplicatedEntitiesVideos)
            {
                return new(true)
                {
                    ResponseBody = errorTranslator.Translate(ValidationErrorCodes.TerritoryStories.DuplicatedVideos),
                };
            }

            // Validate if the videos exist
            foreach (var videoUrl in videosUrls)
            {
                var videoExists = await videoHelperService.VideoExistsAsync(videoUrl.ToString(), ct);

                if (!videoExists)
                {
                    return new(true)
                    {
                        ResponseBody = errorTranslator.Translate(ValidationErrorCodes.TerritoryStoryVideos.NotFound, data: videoUrl),
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

        return new()
        {
            ResponseBody = entityData,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> UpdateAsync(int id, string userName, TerritoryStoryDto entityData, CancellationToken ct = default)
    {
        // Validate user permissions
        var authorizedUserAction = await entityRepository.AuthorizedEntityModifyAsync(id, userName, ct);

        if (!authorizedUserAction)
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
        var hasDuplicatedEntities = await entityRepository.IsDuplicatedAsync(id, entity.Title, ct);

        if (hasDuplicatedEntities)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.TerritoryStories.Duplicated),
            };
        }

        // Update entity data
        mapper.Update(entity, entityData);

        await entityRepository.UpdateAsync(entity, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Update, "Updated territory story", "{@EntityData}", entityData);

        return new()
        {
            ResponseBody = entityData,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> LikeActionAsync(TerritoryStoryLikeDto entityData, CancellationToken ct = default)
    {
        // Validate entity
        var entity = await entityRepository.GetByIdAsync(entityData.TerritoryStoryId, ct);

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

        return new();
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> FeaturedContentActionAsync(int id, string userName, CancellationToken ct = default)
    {
        // Validate user permissions
        var entity = await entityRepository.GetByIdAsync(id, ct);
        var initiativeId = entity?.InitiativeId ?? 0;

        var authorizedUserAction = await initiativeUserRepository.AnyByInitiativeUserAndLevelAsync(initiativeId, userName, (int)InitiativeUserLevelEnum.Leader, ct);

        if (!authorizedUserAction)
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

        if (!entity.Enabled)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.ElementDisabled),
            };
        }

        // Mark territory story as featured content
        entity = await entityRepository.MarkAsFeaturedContentAsync(id, ct);

        if (entity == null)
        {
            return new(true)
            {
                StatusCode = HttpStatusCode.InternalServerError,
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.DatabaseError),
            };
        }

        var entityData = mapper.Map(entity);

        logger.AddLog(LogType.Update, "Marked territory story as featured content", "{@EntityData}", entityData);

        return new()
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
        // Validate user permissions
        var authorizedUserAction = await entityRepository.AuthorizedEntityModifyAsync(id, userName, ct);

        if (!authorizedUserAction)
        {
            return new(true)
            {
                StatusCode = HttpStatusCode.Forbidden,
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

        entity.Enabled = !disable;
        await entityRepository.UpdateAsync(entity, ct);

        var entityData = mapper.Map(entity);

        logger.AddLog(LogType.Update, $"{(disable ? "Disabled" : "Enabled")} territory story", "{@EntityData}", entityData);

        return new()
        {
            ResponseBody = entityData,
        };
    }
}
