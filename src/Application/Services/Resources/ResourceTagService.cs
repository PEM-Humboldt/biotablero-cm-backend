namespace IAVH.BioTablero.CM.Application.Services.Resources;

using System.Net;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Resources;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Resources;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Tags;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

/// <summary>
/// Resource tag service.
/// </summary>
public class ResourceTagService : IResourceTagService
{
    private readonly IResourceTagRepository entityRepository;
    private readonly IValidationErrorTranslator errorTranslator;
    private readonly ILogger logger;
    private readonly IResourceRepository resourceRepository;
    private readonly ITagRepository tagRepository;
    private readonly IResourceService resourceService;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="errorTranslator">Error translator.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="resourceRepository">Resource repository.</param>
    /// <param name="tagRepository">Tag repository.</param>
    /// <param name="resourceService">Resource service.</param>
    public ResourceTagService(
        IResourceTagRepository entityRepository,
        IValidationErrorTranslator errorTranslator,
        ILogger logger,
        IResourceRepository resourceRepository,
        ITagRepository tagRepository,
        IResourceService resourceService)
    {
        this.entityRepository = entityRepository;
        this.errorTranslator = errorTranslator;
        this.logger = logger;
        this.resourceRepository = resourceRepository;
        this.tagRepository = tagRepository;
        this.resourceService = resourceService;
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> AddAsync(string userName, int resourceId, int tagId, CancellationToken ct = default)
    {
        // Validate user permissions
        var authorizedUserAction = await resourceRepository.UserRelationshipExistsAsync(resourceId, userName, ct);

        if (!authorizedUserAction)
        {
            return new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.Forbidden,
            };
        }

        // Validate resource
        var resource = await resourceRepository.GetByIdAsync(resourceId, ct);

        if (resource == null)
        {
            return new CustomWebResponse(true)
            {
                Message = "Resource not found",
            };
        }

        // Validate tag
        var tagExists = await tagRepository.AnyAsync(tagId, ct);

        if (!tagExists)
        {
            return new CustomWebResponse(true)
            {
                Message = "Tag not found",
            };
        }

        // Validate duplicated entities
        var hasDuplicatedEntities = await entityRepository.IsDuplicatedAsync(resourceId, tagId, ct);

        if (hasDuplicatedEntities)
        {
            return new CustomWebResponse(true)
            {
                Message = "Duplicated resource tag relationship",
            };
        }

        // Build entity data
        var entity = new ResourceTag()
        {
            ResourceId = resourceId,
            TagId = tagId,
        };

        // Save data
        entity = await entityRepository.AddAsync(entity, ct);

        // Send email
        await resourceService.SendUpdateNotificationAsync(resource, userName, ct);

        logger.AddLog(LogType.Create, "Added resource tag relationship", "{@EntityData}", entity);

        return new CustomWebResponse();
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
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.ElementNotFound),
            };
        }

        // Validate user permissions
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

        logger.AddLog(LogType.Delete, "Deleted resource tag relationship", "{@EntityData}", entity);

        return new CustomWebResponse();
    }
}
