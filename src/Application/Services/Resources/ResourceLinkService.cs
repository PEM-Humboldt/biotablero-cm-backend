namespace IAVH.BioTablero.CM.Application.Services.Resources;

using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.DTOs.Resources;
using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Resources;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Resources;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

/// <summary>
/// Resource link service.
/// </summary>
public class ResourceLinkService : ServiceRead<ResourceLink, ResourceLinkDto, int>, IResourceLinkService
{
    private const int MaxItemsPerResource = 5;
    private new readonly IResourceLinkRepository entityRepository;
    private readonly IValidator<ResourceLinkDto> entityValidator;
    private readonly ILogger logger;
    private new readonly IMapperCreateReadAndUpdate<ResourceLink, ResourceLinkDto> mapper;
    private readonly IResourceRepository resourceRepository;
    private readonly IWebHelperService webHelperService;
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
    /// <param name="webHelperService">Web Helper service.</param>
    /// <param name="resourceService">Resource service.</param>
    public ResourceLinkService(
        IResourceLinkRepository entityRepository,
        IMapperCreateReadAndUpdate<ResourceLink, ResourceLinkDto> mapper,
        IValidationErrorTranslator errorTranslator,
        IValidator<ResourceLinkDto> entityValidator,
        ILogger logger,
        IResourceRepository resourceRepository,
        IWebHelperService webHelperService,
        IResourceService resourceService)
        : base(entityRepository, mapper, errorTranslator)
    {
        this.entityRepository = entityRepository;
        this.mapper = mapper;
        this.entityValidator = entityValidator;
        this.logger = logger;
        this.resourceRepository = resourceRepository;
        this.webHelperService = webHelperService;
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
    public async Task<CustomWebResponse> AddAsync(string userName, ResourceLinkDto entityData, CancellationToken ct = default)
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
        var resource = await resourceRepository.GetByIdAsync(entityData.ResourceId.Value, ct);

        if (resource == null)
        {
            return new CustomWebResponse(true)
            {
                Message = "Resource not found",
            };
        }

        // Validate user permissions
        var authorizedUserAction = await resourceRepository.UserRelationshipExistsAsync(entityData.ResourceId.Value, userName, ct);

        if (!authorizedUserAction)
        {
            return new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.Forbidden,
            };
        }

        // Validate number of items
        if (resource.Links.Count >= MaxItemsPerResource)
        {
            return new CustomWebResponse(true)
            {
                Message = $"The number of items per resource must be less than or equal to {MaxItemsPerResource}",
            };
        }

        // Validate duplicated entities
        var hasDuplicatedEntities = await entityRepository.IsDuplicatedAsync(new Uri(entityData.Url), ct);

        if (hasDuplicatedEntities)
        {
            return new CustomWebResponse(true)
            {
                Message = "There is already a link with the same URL",
            };
        }

        // Validate if the link exists
        var linkExists = await webHelperService.LinkExistsAsync(entityData.Url, ct);

        if (!linkExists)
        {
            return new CustomWebResponse(true)
            {
                Message = "The URL does not exist",
            };
        }

        // Build entity data
        var entity = mapper.Map(entityData);

        // Save data
        entity = await entityRepository.AddAsync(entity, ct);

        // Send email
        await resourceService.SendUpdateNotificationAsync(resource, userName, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Create, "Added resource link", "{@EntityData}", entityData);

        return new CustomWebResponse()
        {
            ResponseBody = entityData,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> UpdateAsync(int id, string userName, ResourceLinkDto entityData, CancellationToken ct = default)
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
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.GeneralElementNotFound),
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

        // Validate duplicated entities
        var hasDuplicatedEntities = await entityRepository.IsDuplicatedAsync(id, new Uri(entityData.Url), ct);

        if (hasDuplicatedEntities)
        {
            return new CustomWebResponse(true)
            {
                Message = "There is already a link with the same URL",
            };
        }

        // Validate if the link exists
        var linkExists = await webHelperService.LinkExistsAsync(entityData.Url, ct);

        if (!linkExists)
        {
            return new CustomWebResponse(true)
            {
                Message = "The URL does not exist",
            };
        }

        // Update entity data
        mapper.Update(entity, entityData);

        await entityRepository.UpdateAsync(entity, ct);

        // Send email
        var resource = await resourceRepository.GetByIdAsync(entity.ResourceId, ct);
        await resourceService.SendUpdateNotificationAsync(resource, userName, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Update, "Updated resource link", "{@EntityData}", entityData);

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

        var entityData = mapper.Map(entity);

        logger.AddLog(LogType.Delete, "Deleted resource link", "{@EntityData}", entityData);

        return new CustomWebResponse();
    }
}
