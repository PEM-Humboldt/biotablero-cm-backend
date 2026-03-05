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
using IAVH.BioTablero.CM.Application.Interfaces.Services.General;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Resources;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;
using IAVH.BioTablero.CM.Core.Domain.Models.Email;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;
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
    private new readonly IMapperCreateReadAndUpdate<Resource, ResourceDto> mapper;
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
    /// <param name="errorTranslator">Error translator.</param>
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
        IMapperCreateReadAndUpdate<Resource, ResourceDto> mapper,
        IValidationErrorTranslator errorTranslator,
        IValidator<ResourceDto> entityValidator,
        ILogger logger,
        IInitiativeRepository initiativeRepository,
        IInitiativeUserRepository initiativeUserRepository,
        IRepository<ResourceType, int> resourceTypeRepository,
        IResourceLikeRepository resourceLikeRepository,
        IWebViewTools webViewTools,
        IEmailService emailService,
        IIamService iamService)
        : base(entityRepository, mapper, errorTranslator)
    {
        this.entityRepository = entityRepository;
        this.mapper = mapper;
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
                return new(true)
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
            ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.ElementNotFound),
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
        catch (ODataException)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.OdataInvalidFilter),
            };
        }
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> AddAsync(string userName, ResourceDto entityData, CancellationToken ct = default)
    {
        // Validate user level and permissions
        var authorizedUserAction = await initiativeUserRepository.AnyByInitiativeUserAndLevelAsync(entityData.InitiativeId.Value, userName, null, ct);

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

        // Validate initiative
        var initiativeExists = await initiativeRepository.AnyAsync(entityData.InitiativeId.Value, ct);

        if (!initiativeExists)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Initiatives.NotFound),
            };
        }

        // Validate Resource Type
        var resourceTypeExists = await resourceTypeRepository.AnyAsync(entityData.ResourceType.Id.Value, ct);

        if (!resourceTypeExists)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.ResourceTypes.NotFound),
            };
        }

        // Validate duplicated entities
        var hasDuplicatedEntities = await entityRepository.IsDuplicatedAsync(entityData.Name, ct);

        if (hasDuplicatedEntities)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Resources.Duplicated),
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

        return new()
        {
            ResponseBody = entityData,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> UpdateAsync(int id, string userName, ResourceDto entityData, CancellationToken ct = default)
    {
        // Validate user level and permissions
        var authorizedUserAction = await entityRepository.UserRelationshipExistsAsync(id, userName, ct);

        if (!authorizedUserAction)
        {
            return new(true)
            {
                StatusCode = HttpStatusCode.Forbidden,
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

        // Validate entity
        var entity = await entityRepository.GetByIdAsync(id, ct);

        if (entity == null)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.ElementNotFound),
            };
        }

        // Validate duplicated entities
        var hasDuplicatedEntities = await entityRepository.IsDuplicatedAsync(id, entityData.Name, ct);

        if (hasDuplicatedEntities)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Resources.Duplicated),
            };
        }

        // Validate Resource Type
        var resourceTypeExists = await resourceTypeRepository.AnyAsync(entityData.ResourceType.Id.Value, ct);

        if (!resourceTypeExists)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.ResourceTypes.NotFound),
            };
        }

        // Update entity data
        mapper.Update(entity, entityData);

        await entityRepository.UpdateAsync(entity, ct);

        // Send email
        await SendUpdateNotificationAsync(entity, userName, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Update, "Updated resource", "{@EntityData}", entityData);

        return new()
        {
            ResponseBody = entityData,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> LikeActionAsync(ResourceLikeDto entityData, CancellationToken ct = default)
    {
        // Validate entity
        var entity = await entityRepository.GetByIdAsync(entityData.ResourceId, ct);

        if (entity == null)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.ElementNotFound),
            };
        }

        if (entity.IsDraft)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.ElementDisabled),
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

        return new();
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> DeleteAsync(int id, string userName, CancellationToken ct = default)
    {
        // Validate user level and permissions
        var authorizedUserAction = await entityRepository.UserRelationshipExistsAsync(id, userName, ct);

        if (!authorizedUserAction)
        {
            return new(true)
            {
                StatusCode = HttpStatusCode.Forbidden,
            };
        }

        // Validate entity
        var entity = await entityRepository.GetByIdAsync(id, ct);

        if (entity == null)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.ElementNotFound),
            };
        }

        await entityRepository.DeleteAsync(entity, ct);

        var entityData = mapper.Map(entity);

        logger.AddLog(LogType.Delete, "Deleted resource", "{@EntityData}", entityData);

        return new();
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
