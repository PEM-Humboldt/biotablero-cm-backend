namespace IAVH.BioTablero.CM.Application.Services.Resources;

using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Resources;
using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.Services.General;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Resources;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;
using IAVH.BioTablero.CM.Core.Domain.Models.Email;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Resources;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.OData;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

/// <summary>
/// Resource service.
/// </summary>
public class ResourceService : ServiceRead<Resource, ResourceDto, int>, IResourceService
{
    private new readonly IResourceRepository entityRepository;
    private readonly IValidator<ResourceDto> entityValidator;
    private readonly ILogger logger;
    private readonly IInitiativeRepository initiativeRepository;
    private readonly IInitiativeUserRepository initiativeUserRepository;
    private readonly IRepository<ResourceType, int> resourceTypeRepository;
    private readonly IResourceLikeRepository resourceLikeRepository;
    private readonly IWebViewTools webViewTools;
    private readonly IEmailService emailService;
    private readonly IIamService iamService;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="entityValidator">Entity validator.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="initiativeRepository">Initiative repository.</param>
    /// <param name="initiativeUserRepository">Initiative User repository.</param>
    /// <param name="resourceTypeRepository">Resource Type repository.</param>
    /// <param name="resourceLikeRepository">Resource Like repository.</param>
    /// <param name="webViewTools">Web View Tools.</param>
    /// <param name="emailService">Email service.</param>
    /// <param name="iamService">IAM service.</param>
    public ResourceService(
        IResourceRepository entityRepository,
        IMapper<Resource, ResourceDto> mapper,
        IValidator<ResourceDto> entityValidator,
        ILogger logger,
        IInitiativeRepository initiativeRepository,
        IInitiativeUserRepository initiativeUserRepository,
        IRepository<ResourceType, int> resourceTypeRepository,
        IResourceLikeRepository resourceLikeRepository,
        IWebViewTools webViewTools,
        IEmailService emailService,
        IIamService iamService)
        : base(entityRepository, mapper)
    {
        this.entityRepository = entityRepository;
        this.entityValidator = entityValidator;
        this.logger = logger;
        this.initiativeRepository = initiativeRepository;
        this.initiativeUserRepository = initiativeUserRepository;
        this.resourceTypeRepository = resourceTypeRepository;
        this.resourceLikeRepository = resourceLikeRepository;
        this.webViewTools = webViewTools;
        this.emailService = emailService;
        this.iamService = iamService;
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetItemAsync(int id, string userName, CancellationToken ct = default)
    {
        // Validate user level and permissions
        var entity = await entityRepository.GetByIdAsync(id, ct);

        if (entity != null)
        {
            var userBelongsToInitiative = await entityRepository.UserRelationshipExistsAsync(id, userName, ct);

            if (!userBelongsToInitiative && entity.IsDraft)
            {
                return new CustomWebResponse(true)
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
            Message = MessageConstants.NotFound,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetByInitiativeAsync(int initiativeId, string userName, ODataQueryOptions<Resource> queryOptions, CancellationToken ct = default)
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
        catch (ODataException ex)
        {
            return new(true)
            {
                Message = $"Invalid filter: {ex.Message}",
            };
        }
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> AddAsync(string userName, ResourceDto entityData, CancellationToken ct = default)
    {
        // Validate user permissions
        var authorizedUserAction = await initiativeUserRepository.AnyByInitiativeUserAndLevelAsync(entityData.InitiativeId.Value, userName, null, ct);

        if (!authorizedUserAction)
        {
            return new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.Forbidden,
            };
        }

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

        // Validate initiative
        var initiativeExists = await initiativeRepository.AnyAsync(entityData.InitiativeId.Value, ct);

        if (!initiativeExists)
        {
            return new CustomWebResponse(true)
            {
                Message = "Initiative not found",
            };
        }

        // Validate Resource Type
        var resourceTypeExists = await resourceTypeRepository.AnyAsync(entityData.ResourceType.Id.Value, ct);

        if (!resourceTypeExists)
        {
            return new CustomWebResponse(true)
            {
                Message = "Resource Type not found",
            };
        }

        // Validate duplicated entities
        var hasDuplicatedEntities = await entityRepository.IsDuplicatedAsync(entityData.Name, ct);

        if (hasDuplicatedEntities)
        {
            return new CustomWebResponse(true)
            {
                Message = "There is already a resource with the same name",
            };
        }

        // Build entity data
        var entity = mapper.Map(entityData);

        var now = DateTime.Now;
        entity.CreationDate = now;

        if (!entity.IsDraft)
        {
            entity.PublicationDate = now;
        }

        // Save data
        entity = await entityRepository.AddAsync(entity, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Create, "Added resource", "{@EntityData}", entityData);

        return new CustomWebResponse()
        {
            ResponseBody = entityData,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> UpdateAsync(int id, string userName, ResourceDto entityData, CancellationToken ct = default)
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
        var authorizedUserAction = await entityRepository.UserRelationshipExistsAsync(id, userName, ct);

        if (!authorizedUserAction)
        {
            return new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.Forbidden,
            };
        }

        // Validate duplicated entities
        var hasDuplicatedEntities = await entityRepository.IsDuplicatedAsync(id, entity.Name, ct);

        if (hasDuplicatedEntities)
        {
            return new CustomWebResponse(true)
            {
                Message = "There is already a resource with the same name",
            };
        }

        // Validate Resource Type
        var resourceTypeExists = await resourceTypeRepository.AnyAsync(entityData.ResourceType.Id.Value, ct);

        if (!resourceTypeExists)
        {
            return new CustomWebResponse(true)
            {
                Message = "Resource Type not found",
            };
        }

        // Update entity data
        if (!entity.IsDraft)
        {
            entity.PublicationDate = DateTime.Now;
        }

        await entityRepository.UpdateAsync(entity, ct);

        // Send email
        await SendUpdateNotificationAsync(entity, userName, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Update, "Updated resource", "{@EntityData}", entityData);

        return new CustomWebResponse()
        {
            ResponseBody = entityData,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> LikeActionAsync(ResourceLikeDto entityData, CancellationToken ct = default)
    {
        // Validate Territory Story
        var entity = await entityRepository.GetByIdAsync(entityData.ResourceId, ct);

        if (entity == null)
        {
            return new CustomWebResponse(true)
            {
                Message = MessageConstants.NotFound,
            };
        }

        if (entity.IsDraft)
        {
            return new CustomWebResponse(true)
            {
                Message = MessageConstants.DisabledElement,
            };
        }

        var hasDuplicatedEntities = await resourceLikeRepository.IsDuplicatedAsync(entityData.ResourceId, entityData.UserName, ct);

        if (hasDuplicatedEntities)
        {
            var like = await resourceLikeRepository.GetByResourceAndUserNameAsync(entityData.ResourceId, entityData.UserName, ct);
            await resourceLikeRepository.DeleteAsync(like, ct);
            logger.AddLog(LogType.Delete, $"Unliked resource", "{@EntityData}", entityData);
        }
        else
        {
            var like = new ResourceLike()
            {
                CreationDate = DateTime.Now,
                ResourceId = entityData.ResourceId,
                UserName = entityData.UserName,
            };

            await resourceLikeRepository.AddAsync(like, ct);
            logger.AddLog(LogType.Create, $"Liked resource", "{@EntityData}", entityData);
        }

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
                Message = MessageConstants.NotFound,
            };
        }

        // Validate user permissions
        var authorizedUserAction = await entityRepository.UserRelationshipExistsAsync(id, userName, ct);

        if (!authorizedUserAction)
        {
            return new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.Forbidden,
            };
        }

        await entityRepository.DeleteAsync(entity, ct);

        var entityData = mapper.Map(entity);

        logger.AddLog(LogType.Delete, "Deleted resource", "{@EntityData}", entityData);

        return new CustomWebResponse();
    }

    /// <inheritdoc/>
    public async Task<bool> SendUpdateNotificationAsync(Resource resource, string userName, CancellationToken ct = default)
    {
        if (resource.IsDraft)
        {
            return true;
        }

        var emailData = new UpdateResourceEmailData
        {
            ResourceName = resource.Name,
            EditorUserName = userName,
        };

        var initiativeUsers = (await initiativeUserRepository.GetByInitiativeAsync(resource.InitiativeId, ct))
            .Select(e => e.UserName)
            .ToArray();

        var externalUsersData = await iamService.GetUsersDataAsync(initiativeUsers, ct);

        var receivers = externalUsersData
            .Select(e => new CustomEmailAddress(e.FullName, e.Email))
            .ToArray();

        var htmlBody = await webViewTools.RenderViewToStringAsync("UpdateResource", emailData);

        var response = await emailService.SendEmailAsync(emailData.Subject, receivers, null, htmlBody, ct);

        return !string.IsNullOrEmpty(response);
    }
}
