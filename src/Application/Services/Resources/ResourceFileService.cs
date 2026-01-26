namespace IAVH.BioTablero.CM.Application.Services.Resources;

using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.DTOs.Resources;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Resources;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;
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
    private new readonly IMapperCreateAndRead<ResourceFile, ResourceFileDto> mapper;
    private readonly IResourceRepository resourceRepository;
    private readonly IResourceService resourceService;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="entityValidator">Entity validator.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="resourceRepository">Resource repository.</param>
    /// <param name="resourceService">Resource service.</param>
    public ResourceFileService(
        IResourceFileRepository entityRepository,
        IMapperCreateAndRead<ResourceFile, ResourceFileDto> mapper,
        IValidator<ResourceFileDto> entityValidator,
        ILogger logger,
        IResourceRepository resourceRepository,
        IResourceService resourceService)
        : base(entityRepository, mapper)
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

        // Validate resource
        var resource = await resourceRepository.GetByIdAsync(entityData.ResourceId, ct);

        if (resource == null)
        {
            return new CustomWebResponse(true)
            {
                Message = "Resource not found",
            };
        }

        // Validate user level and permissions
        var authorizedUserAction = await resourceRepository.UserRelationshipExistsAsync(entityData.ResourceId, userName, ct);

        if (!authorizedUserAction)
        {
            return new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.Forbidden,
            };
        }

        // Validate number of items
        if (resource.Files.Count >= MaxItemsPerResource)
        {
            return new CustomWebResponse(true)
            {
                Message = $"The number of items per resource must be less than or equal to {MaxItemsPerResource}",
            };
        }

        // Validate file
        if (formFile.IsEmpty())
        {
            return new CustomWebResponse(true)
            {
                Message = "The file is empty",
            };
        }

        if (!formFile.IsValidResourceFile())
        {
            return new CustomWebResponse(true)
            {
                Message = "Invalid file format",
            };
        }

        if (!formFile.HasResourceFileValidSize())
        {
            return new CustomWebResponse(true)
            {
                Message = "Invalid file size",
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

        return new CustomWebResponse()
        {
            ResponseBody = entityData,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> UpdateAsync(int id, string userName, ResourceFileDto entityData, IInputFile formFile, CancellationToken ct = default)
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

        // Validate file
        var updateHasFile = !formFile.IsEmpty();
        if (updateHasFile)
        {
            if (!formFile.IsValidResourceFile())
            {
                return new CustomWebResponse(true)
                {
                    Message = "Invalid file format",
                };
            }

            if (!formFile.HasResourceFileValidSize())
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
        var authorizedUserAction = await resourceRepository.UserRelationshipExistsAsync(entity.ResourceId, userName, ct);

        if (!authorizedUserAction)
        {
            return new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.Forbidden,
            };
        }

        // Update entity data
        entity.Name = entityData.Name;

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
        var authorizedUserAction = await resourceRepository.UserRelationshipExistsAsync(entity.ResourceId, userName, ct);

        if (!authorizedUserAction)
        {
            return new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.Forbidden,
            };
        }

        await entityRepository.DeleteAsync(entity, ct);

        // Send email
        var resource = await resourceRepository.GetByIdAsync(entity.ResourceId, ct);
        await resourceService.SendUpdateNotificationAsync(resource, userName, ct);

        var entityData = mapper.Map(entity);

        logger.AddLog(LogType.Delete, "Deleted resource file", "{@EntityData}", entityData);

        return new CustomWebResponse();
    }
}
