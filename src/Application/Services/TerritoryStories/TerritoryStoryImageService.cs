namespace IAVH.BioTablero.CM.Application.Services.TerritoryStories;

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
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;
using IAVH.BioTablero.CM.Core.Interfaces.ExternalServices;
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
    private new readonly IMapperCreateReadAndUpdate<TerritoryStoryImage, TerritoryStoryImageDto> mapper;
    private readonly ITerritoryStoryRepository territoryStoryRepository;
    private readonly IStorageService storageService;
    private readonly IImageUtilsService imageUtilsService;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="errorTranslator">Error translator.</param>
    /// <param name="entityValidator">Entity validator.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="territoryStoryRepository">Territory Story repository.</param>
    /// <param name="storageService">Storage service.</param>
    /// <param name="imageUtilsService">Image Utils service.</param>
    public TerritoryStoryImageService(
        ITerritoryStoryImageRepository entityRepository,
        IMapperCreateReadAndUpdate<TerritoryStoryImage, TerritoryStoryImageDto> mapper,
        IValidationErrorTranslator errorTranslator,
        IValidator<TerritoryStoryImageDto> entityValidator,
        ILogger logger,
        ITerritoryStoryRepository territoryStoryRepository,
        IStorageService storageService,
        IImageUtilsService imageUtilsService)
        : base(entityRepository, mapper, errorTranslator)
    {
        this.entityRepository = entityRepository;
        this.mapper = mapper;
        this.entityValidator = entityValidator;
        this.logger = logger;
        this.territoryStoryRepository = territoryStoryRepository;
        this.storageService = storageService;
        this.imageUtilsService = imageUtilsService;
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

        // Compress and convert image
        using var originalImageStream = formFile.OpenStream();
        var compressedImageStream = await imageUtilsService.CompressToWebpAsync(originalImageStream, 75, ct);

        if (compressedImageStream == null)
        {
            return new CustomWebResponse(true)
            {
                Message = "Image processing error",
            };
        }

        // Build entity data
        var entity = mapper.Map(entityData);
        entity.FeaturedContent = false;

        // Save data
        entity = await entityRepository.AddAsync(entity, compressedImageStream, MediaTypeNames.Image.ImageWebp, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Create, "Added territory story image", "{@EntityData}", entityData);

        return new CustomWebResponse()
        {
            ResponseBody = entityData,
        };
    }

    /// <inheritdoc/>
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
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.GeneralElementNotFound),
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

        // Update entity data
        mapper.Update(entity, entityData);

        if (!updateHasFile)
        {
            await entityRepository.UpdateAsync(entity, ct);
        }
        else
        {
            // Compress and convert image
            using var originalImageStream = formFile.OpenStream();
            var compressedImageStream = await imageUtilsService.CompressToWebpAsync(originalImageStream, 75, ct);

            if (compressedImageStream == null)
            {
                return new CustomWebResponse(true)
                {
                    Message = "Image processing error",
                };
            }

            await entityRepository.UpdateAsync(entity, compressedImageStream, MediaTypeNames.Image.ImageWebp, ct);
        }

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Update, "Updated territory story image", "{@EntityData}", entityData);

        return new CustomWebResponse()
        {
            ResponseBody = entityData,
        };
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
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.GeneralElementNotFound),
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

        // Mark territory story image as featured content
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

        logger.AddLog(LogType.Update, "Marked territory story image as featured content", "{@EntityData}", entityData);

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
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.GeneralElementNotFound),
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
        await storageService.DeleteFileAsync(entity.FileUrl.ToString(), ct);

        var entityData = mapper.Map(entity);

        logger.AddLog(LogType.Delete, "Deleted territory story image", "{@EntityData}", entityData);

        return new CustomWebResponse();
    }
}
