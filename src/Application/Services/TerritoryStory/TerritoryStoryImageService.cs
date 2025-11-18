namespace IAVH.BioTablero.CM.Application.Services.TerritoryStory;

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
using IAVH.BioTablero.CM.Core.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.TerritoryStories;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

/// <summary>
/// Territory Story Image service.
/// </summary>
public class TerritoryStoryImageService : ServiceRead<TerritoryStoryImage, TerritoryStoryImageDto, int>, ITerritoryStoryImageService
{
    private new readonly ITerritoryStoryImageRepository entityRepository;
    private readonly IValidator<TerritoryStoryImageDto> entityValidator;
    private readonly ILogger logger;
    private readonly ITerritoryStoryRepository territoryStoryRepository;
    private readonly IInitiativeUserRepository initiativeUserRepository;
    private readonly IStorageService storageService;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="entityValidator">Entity validator.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="territoryStoryRepository">Territory Story repository.</param>
    /// <param name="initiativeUserRepository">Initiative User repository.</param>
    /// <param name="storageService">Storage service.</param>
    public TerritoryStoryImageService(
        ITerritoryStoryImageRepository entityRepository,
        IMapper<TerritoryStoryImage, TerritoryStoryImageDto> mapper,
        IValidator<TerritoryStoryImageDto> entityValidator,
        ILogger logger,
        ITerritoryStoryRepository territoryStoryRepository,
        IInitiativeUserRepository initiativeUserRepository,
        IStorageService storageService)
        : base(entityRepository, mapper)
    {
        this.entityRepository = entityRepository;
        this.entityValidator = entityValidator;
        this.logger = logger;
        this.territoryStoryRepository = territoryStoryRepository;
        this.initiativeUserRepository = initiativeUserRepository;
        this.storageService = storageService;
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
    /// <param name="userName">User name.</param>
    /// <param name="entityData">Entity data.</param>
    /// <param name="formFile">Image data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> AddAsync(string userName, TerritoryStoryImageDto entityData, IInputFile formFile, CancellationToken ct = default)
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

        // Validate user level and permissions
        var authorizedUserAction = await territoryStoryRepository.AuthorizedUserAction(entityData.TerritoryStoryId, userName, ct);

        if (!authorizedUserAction)
        {
            return new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.Forbidden,
            };
        }

        // Validate image
        if (formFile.IsEmpty())
        {
            return new CustomWebResponse(true)
            {
                Message = "The file is empty",
            };
        }

        if (!formFile.IsValidImage())
        {
            return new CustomWebResponse(true)
            {
                Message = "Invalid file format",
            };
        }

        if (!formFile.HasTerritoryStoryImageValidSize())
        {
            return new CustomWebResponse(true)
            {
                Message = "Invalid file size",
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
        var hasDuplicatedEntities = await entityRepository.IsDuplicatedAsync(entityData.FileUrl, ct);

        if (hasDuplicatedEntities)
        {
            return new CustomWebResponse(true)
            {
                Message = "There is already an image with the same URL",
            };
        }

        // Build entity data
        var entity = mapper.Map(entityData);
        entity.FeaturedContent = false;

        // Save data
        entity = await entityRepository.AddAsync(entity, formFile, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Create, "Added territory story image", "{@EntityData}", entityData);

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
    /// <param name="formFile">Image data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> UpdateAsync(int id, string userName, TerritoryStoryImageDto entityData, IInputFile formFile, CancellationToken ct = default)
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

        // Validate image
        var updateHasFile = !formFile.IsEmpty();
        if (updateHasFile)
        {
            if (!formFile.IsValidImage())
            {
                return new CustomWebResponse(true)
                {
                    Message = "Invalid file format",
                };
            }

            if (!formFile.HasTerritoryStoryImageValidSize())
            {
                return new CustomWebResponse(true)
                {
                    Message = "Invalid file size",
                };
            }
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
        var authorizedUserAction = await entityRepository.AuthorizedUserAction(id, userName, ct);

        if (!authorizedUserAction)
        {
            return new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.Forbidden,
            };
        }

        // Validate duplicated entities
        var hasDuplicatedEntities = await entityRepository.IsDuplicatedAsync(id, entity.FileUrl, ct);

        if (hasDuplicatedEntities)
        {
            return new CustomWebResponse(true)
            {
                Message = "There is already an image with the same URL",
            };
        }

        // Update entity data
        entity.Description = entity.Description;
        if (!updateHasFile)
        {
            await entityRepository.UpdateAsync(entity, ct);
        }
        else
        {
            await entityRepository.UpdateAsync(entity, formFile, ct);
        }

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Update, "Updated territory story image", "{@EntityData}", entityData);

        return new CustomWebResponse()
        {
            ResponseBody = entityData,
        };
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
        // Validate user level and permissions
        var authorizedUserAction = await entityRepository.AuthorizedUserAction(id, userName, ct);

        if (!authorizedUserAction)
        {
            return new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.Forbidden,
            };
        }

        // Validate Territory Story Image
        var territoryStoryImageExists = await entityRepository.AnyAsync(id, ct);

        if (!territoryStoryImageExists)
        {
            return new CustomWebResponse(true)
            {
                Message = "Territory Story Image not found",
            };
        }

        // Mark territory story image as featured content
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

        logger.AddLog(LogType.Update, "Marked territory story image as featured content", "{@EntityData}", entityData);

        return new CustomWebResponse()
        {
            ResponseBody = entityData,
        };
    }

    /// <summary>
    /// Delete element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> DeleteAsync(int id, string userName, CancellationToken ct = default)
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

        await entityRepository.DeleteAsync(entity, ct);

        var entityData = mapper.Map(entity);

        logger.AddLog(LogType.Delete, "Deleted territory story image", "{@EntityData}", entityData);

        return new CustomWebResponse();
    }
}
