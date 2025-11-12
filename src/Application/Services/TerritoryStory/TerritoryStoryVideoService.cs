namespace IAVH.BioTablero.CM.Application.Services.TerritoryStory;

using System;
using System.Linq;
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

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="entityValidator">Entity validator.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="territoryStoryRepository">Territory Story repository.</param>
    public TerritoryStoryVideoService(
        ITerritoryStoryVideoRepository entityRepository,
        IMapper<TerritoryStoryVideo, TerritoryStoryVideoDto> mapper,
        IValidator<TerritoryStoryVideoDto> entityValidator,
        ILogger logger,
        ITerritoryStoryRepository territoryStoryRepository)
        : base(entityRepository, mapper)
    {
        this.entityRepository = entityRepository;
        this.entityValidator = entityValidator;
        this.logger = logger;
        this.territoryStoryRepository = territoryStoryRepository;
    }

    /// <summary>
    /// Get elements by territory story.
    /// </summary>
    /// <param name="territoryStoryId">Territory Story identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities by selected territory story.</returns>
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

    /// <summary>
    /// Add element.
    /// </summary>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> AddAsync(TerritoryStoryVideoDto entityData, CancellationToken ct = default)
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
        var territoryStoryExists = await territoryStoryRepository.AnyAsync(territoryStoryId, ct);

        if (!territoryStoryExists)
        {
            return new CustomWebResponse(true)
            {
                Message = "Territory Story not found",
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

    /// <summary>
    /// Update element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> UpdateAsync(int id, TerritoryStoryVideoDto entityData, CancellationToken ct = default)
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

        // Validate duplicated entities
        var hasDuplicatedEntities = await entityRepository.IsDuplicatedAsync(id, entity.FileUrl, ct);

        if (hasDuplicatedEntities)
        {
            return new CustomWebResponse(true)
            {
                Message = "There is already a video with the same URL",
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

        await entityRepository.DeleteAsync(entity, ct);

        var entityData = mapper.Map(entity);

        logger.AddLog(LogType.Delete, "Deleted territory story video", "{@EntityData}", entityData);

        return new CustomWebResponse();
    }
}
