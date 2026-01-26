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
using IAVH.BioTablero.CM.Application.Interfaces.Services.TerritoryStories;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.TerritoryStories;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

/// <summary>
/// Territory Story Video service.
/// </summary>
public class TerritoryStoryVideoService : ServiceRead<TerritoryStoryVideo, TerritoryStoryVideoDto, int>, ITerritoryStoryVideoService
{
    private new readonly ITerritoryStoryVideoRepository entityRepository;
    private readonly IValidator<TerritoryStoryVideoDto> entityValidator;
    private readonly ILogger logger;
    private readonly ITerritoryStoryRepository territoryStoryRepository;
    private readonly IVideoHelperService videoHelperService;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="entityValidator">Entity validator.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="territoryStoryRepository">Territory Story repository.</param>
    /// <param name="videoHelperService">Video Helper service.</param>
    public TerritoryStoryVideoService(
        ITerritoryStoryVideoRepository entityRepository,
        IMapper<TerritoryStoryVideo, TerritoryStoryVideoDto> mapper,
        IValidator<TerritoryStoryVideoDto> entityValidator,
        ILogger logger,
        ITerritoryStoryRepository territoryStoryRepository,
        IVideoHelperService videoHelperService)
        : base(entityRepository, mapper)
    {
        this.entityRepository = entityRepository;
        this.entityValidator = entityValidator;
        this.logger = logger;
        this.territoryStoryRepository = territoryStoryRepository;
        this.videoHelperService = videoHelperService;
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
    public async Task<CustomWebResponse> GetByTerritoryStoryAsync(int territoryStoryId, CancellationToken ct = default)
    {
        var dataListEntity = await entityRepository.GetByTerritoryStoryAsync(territoryStoryId, ct);

        var dataListDto = dataListEntity
            .Select(mapper.Map);

        return new()
        {
            ResponseBody = dataListDto,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> AddAsync(string userName, TerritoryStoryVideoDto entityData, CancellationToken ct = default)
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

        // Validate territory story
        var territoryStoryId = entityData.TerritoryStoryId ?? 0;
        var territoryStory = await territoryStoryRepository.GetByIdAsync(territoryStoryId, ct);

        if (territoryStory == null)
        {
            return new CustomWebResponse(true)
            {
                Message = "Territory Story not found",
            };
        }

        if (!territoryStory.Enabled)
        {
            return new CustomWebResponse(true)
            {
                Message = "Territory Story disabled",
            };
        }

        // Validate user level and permissions
        var authorizedUserAction = await territoryStoryRepository.AuthorizedEntityModifyAsync(entityData.TerritoryStoryId.Value, userName, ct);

        if (!authorizedUserAction)
        {
            return new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.Forbidden,
            };
        }

        // Validate duplicated entities
        var hasDuplicatedEntities = await entityRepository.IsDuplicatedAsync(new Uri(entityData.FileUrl), ct);

        if (hasDuplicatedEntities)
        {
            return new CustomWebResponse(true)
            {
                Message = "There is already a video with the same URL",
            };
        }

        // Validate if the video exists
        var videoExists = await videoHelperService.VideoExistsAsync(entityData.FileUrl, ct);

        if (!videoExists)
        {
            return new CustomWebResponse(true)
            {
                Message = "The video URL does not exist",
            };
        }

        // Build entity data
        var entity = mapper.Map(entityData);

        // Save data
        entity = await entityRepository.AddAsync(entity, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Create, "Added territory story video", "{@EntityData}", entityData);

        return new CustomWebResponse()
        {
            ResponseBody = entityData,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> UpdateAsync(int id, string userName, TerritoryStoryVideoDto entityData, CancellationToken ct = default)
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

        // Validate user level and permissions
        var authorizedUserAction = await entityRepository.AuthorizedEntityModifyAsync(id, userName, ct);

        if (!authorizedUserAction)
        {
            return new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.Forbidden,
            };
        }

        // Validate territory story
        var territoryStory = await territoryStoryRepository.GetByIdAsync(entity.TerritoryStoryId, ct);

        if (!territoryStory.Enabled)
        {
            return new CustomWebResponse(true)
            {
                Message = "Territory Story disabled",
            };
        }

        // Validate duplicated entities
        var hasDuplicatedEntities = await entityRepository.IsDuplicatedAsync(id, new Uri(entityData.FileUrl), ct);

        if (hasDuplicatedEntities)
        {
            return new CustomWebResponse(true)
            {
                Message = "There is already a video with the same URL",
            };
        }

        // Validate if the video exists
        var videoExists = await videoHelperService.VideoExistsAsync(entityData.FileUrl, ct);

        if (!videoExists)
        {
            return new CustomWebResponse(true)
            {
                Message = "The video URL does not exist",
            };
        }

        // Update entity data
        entity.FileUrl = new Uri(entityData.FileUrl);

        await entityRepository.UpdateAsync(entity, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Update, "Updated territory story video", "{@EntityData}", entityData);

        return new CustomWebResponse()
        {
            ResponseBody = entityData,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> DeleteAsync(int id, string userName, CancellationToken ct = default)
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

        // Validate user level and permissions
        var authorizedUserAction = await entityRepository.AuthorizedEntityModifyAsync(id, userName, ct);

        if (!authorizedUserAction)
        {
            return new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.Forbidden,
            };
        }

        // Validate territory story
        var territoryStory = await territoryStoryRepository.GetByIdAsync(entity.TerritoryStoryId, ct);

        if (!territoryStory.Enabled)
        {
            return new CustomWebResponse(true)
            {
                Message = "Territory Story disabled",
            };
        }

        await entityRepository.DeleteAsync(entity, ct);

        var entityData = mapper.Map(entity);

        logger.AddLog(LogType.Delete, "Deleted territory story video", "{@EntityData}", entityData);

        return new CustomWebResponse();
    }
}
