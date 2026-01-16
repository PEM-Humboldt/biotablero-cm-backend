namespace IAVH.BioTablero.CM.Application.Services.Resources;

using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Resources;
using IAVH.BioTablero.CM.Application.Interfaces.General;
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
    private new readonly IResourceFileRepository entityRepository;
    private readonly IValidator<ResourceFileDto> entityValidator;
    private readonly ILogger logger;
    private readonly IResourceRepository resourceRepository;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="entityValidator">Entity validator.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="resourceRepository">Resource repository.</param>
    public ResourceFileService(
        IResourceFileRepository entityRepository,
        IMapper<ResourceFile, ResourceFileDto> mapper,
        IValidator<ResourceFileDto> entityValidator,
        ILogger logger,
        IResourceRepository resourceRepository)
        : base(entityRepository, mapper)
    {
        this.entityRepository = entityRepository;
        this.entityValidator = entityValidator;
        this.logger = logger;
        this.resourceRepository = resourceRepository;
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

        // Validate user permissions
        var resourceExists = await resourceRepository.AnyAsync(entityData.ResourceId, ct);

        if (!resourceExists)
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
        var authorizedUserAction = await resourceRepository.UserRelationshipExistsAsync(entityData.ResourceId, userName, ct);

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

        var entityData = mapper.Map(entity);

        logger.AddLog(LogType.Delete, "Deleted resource file", "{@EntityData}", entityData);

        return new CustomWebResponse();
    }
}
