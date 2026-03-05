namespace IAVH.BioTablero.CM.Application.Services.Resources;

using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.DTOs.Resources;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Resources;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;
using IAVH.BioTablero.CM.Core.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Resources;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

/// <summary>
/// Resource file service.
/// </summary>
public class ResourceFileService : ServiceRead<ResourceFile, ResourceFileDto, int>, IResourceFileService
{
    private const int MaxItemsPerResource = 5;
    private new readonly IResourceFileRepository entityRepository;
    private readonly IValidator<ResourceFileDto> entityValidator;
    private readonly ILogger logger;
    private new readonly IMapperCreateReadAndUpdate<ResourceFile, ResourceFileDto> mapper;
    private readonly IResourceRepository resourceRepository;
    private readonly IResourceService resourceService;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="errorTranslator">Error translator.</param>
    /// <param name="entityValidator">Entity validator.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="resourceRepository">Resource repository.</param>
    /// <param name="resourceService">Resource service.</param>
    public ResourceFileService(
        IResourceFileRepository entityRepository,
        IMapperCreateReadAndUpdate<ResourceFile, ResourceFileDto> mapper,
        IValidationErrorTranslator errorTranslator,
        IValidator<ResourceFileDto> entityValidator,
        ILogger logger,
        IResourceRepository resourceRepository,
        IResourceService resourceService)
        : base(entityRepository, mapper, errorTranslator)
    {
        this.entityRepository = entityRepository;
        this.mapper = mapper;
        this.entityValidator = entityValidator;
        this.logger = logger;
        this.resourceRepository = resourceRepository;
        this.resourceService = resourceService;
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetByResourceAsync(int resourceId, CancellationToken ct = default)
    {
        var dataListEntity = await entityRepository.GetByResourceAsync(resourceId, ct);

        var dataListDto = dataListEntity
            .Select(mapper.Map);

        return new()
        {
            ResponseBody = dataListDto,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> AddAsync(string userName, ResourceFileDto entityData, IInputFile formFile, CancellationToken ct = default)
    {
        // Validate user level and permissions
        var resourceId = entityData?.ResourceId ?? 0;

        var authorizedUserAction = await resourceRepository.AuthorizedEntityModifyAsync(resourceId, userName, ct);

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

        // Validate resource
        var resource = await resourceRepository.GetByIdAsync(entityData.ResourceId, ct);

        if (resource == null)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Resources.NotFound),
            };
        }

        // Validate number of items
        if (resource.Files.Count >= MaxItemsPerResource)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.ResourceFiles.ItemsLimitExceeded, data: MaxItemsPerResource),
            };
        }

        // Validate file
        if (formFile.IsEmpty())
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Files.Empty),
            };
        }

        if (!formFile.IsValidResourceFile())
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Files.InvalidFormat),
            };
        }

        if (!formFile.HasResourceFileValidSize())
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Files.InvalidSize),
            };
        }

        // Build entity data
        var entity = mapper.Map(entityData);

        // Save data
        entity = await entityRepository.AddAsync(entity, formFile, ct);

        // Send email
        await resourceService.SendUpdateNotificationAsync(resource, userName, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Create, "Added resource file", "{@EntityData}", entityData);

        return new()
        {
            ResponseBody = entityData,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> UpdateAsync(int id, string userName, ResourceFileDto entityData, IInputFile formFile, CancellationToken ct = default)
    {
        // Validate user level and permissions
        var entity = await entityRepository.GetByIdAsync(id, ct);
        var resourceId = entity?.ResourceId ?? 0;

        var authorizedUserAction = await resourceRepository.AuthorizedEntityModifyAsync(resourceId, userName, ct);

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

        // Validate data
        var validationResult = await entityValidator.ValidateAsync(entityData, ct);

        if (!validationResult.IsValid)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(validationResult.Errors),
            };
        }

        // Validate file
        var updateHasFile = !formFile.IsEmpty();
        if (updateHasFile)
        {
            if (!formFile.IsValidResourceFile())
            {
                return new(true)
                {
                    ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Files.InvalidFormat),
                };
            }

            if (!formFile.HasResourceFileValidSize())
            {
                return new(true)
                {
                    ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Files.InvalidSize),
                };
            }
        }

        // Update entity data
        mapper.Update(entity, entityData);

        if (!updateHasFile)
        {
            await entityRepository.UpdateAsync(entity, ct);
        }
        else
        {
            await entityRepository.UpdateAsync(entity, formFile, ct);
        }

        // Send email
        var resource = await resourceRepository.GetByIdAsync(entity.ResourceId, ct);
        await resourceService.SendUpdateNotificationAsync(resource, userName, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Update, "Updated resource file", "{@EntityData}", entityData);

        return new()
        {
            ResponseBody = entityData,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> DeleteAsync(int id, string userName, CancellationToken ct = default)
    {
        // Validate user level and permissions
        var entity = await entityRepository.GetByIdAsync(id, ct);
        var resourceId = entity?.ResourceId ?? 0;

        var authorizedUserAction = await resourceRepository.AuthorizedEntityModifyAsync(resourceId, userName, ct);

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

        await entityRepository.DeleteAsync(entity, ct);

        // Send email
        var resource = await resourceRepository.GetByIdAsync(entity.ResourceId, ct);
        await resourceService.SendUpdateNotificationAsync(resource, userName, ct);

        var entityData = mapper.Map(entity);

        logger.AddLog(LogType.Delete, "Deleted resource file", "{@EntityData}", entityData);

        return new();
    }
}
