namespace IAVH.BioTablero.CM.Application.Services.Initiatives;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Utils;
using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Notifications;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Models.Iam;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;

using Microsoft.AspNetCore.OData.Query;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

using InitiativeUserLevelEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.InitiativeUserLevel;
using JoinRequestStatusEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.JoinRequestStatus;

/// <summary>
/// Join Request service.
/// </summary>
public class JoinRequestService : ServiceRead<JoinRequest, JoinRequestDto, int>, IJoinRequestService
{
    private new readonly IJoinRequestRepository entityRepository;
    private readonly IValidator<JoinRequestDto> entityValidator;
    private readonly ILogger logger;
    private new readonly IMapperCreateAndRead<JoinRequest, JoinRequestDto> mapper;
    private readonly IInitiativeRepository initiativeRepository;
    private readonly IInitiativeUserRepository initiativeUserRepository;
    private readonly IIamService iamService;
    private readonly INotificationService notificationService;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="errorTranslator">Error translator.</param>
    /// <param name="entityValidator">Entity validator.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="initiativeRepository">Initiative repository.</param>
    /// <param name="initiativeUserRepository">Initiative user repository.</param>
    /// <param name="iamService">IAM service.</param>
    /// <param name="notificationService">Notification service.</param>
    public JoinRequestService(
        IJoinRequestRepository entityRepository,
        IMapperCreateAndRead<JoinRequest, JoinRequestDto> mapper,
        IValidationErrorTranslator errorTranslator,
        IValidator<JoinRequestDto> entityValidator,
        ILogger logger,
        IInitiativeRepository initiativeRepository,
        IInitiativeUserRepository initiativeUserRepository,
        INotificationService notificationService,
        IIamService iamService)
        : base(entityRepository, mapper, errorTranslator)
    {
        this.entityRepository = entityRepository;
        this.mapper = mapper;
        this.entityValidator = entityValidator;
        this.logger = logger;
        this.initiativeRepository = initiativeRepository;
        this.initiativeUserRepository = initiativeUserRepository;
        this.iamService = iamService;
        this.notificationService = notificationService;
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetListAsync(int initiativeId, string userName, ODataQueryOptions<JoinRequest> queryOptions, CancellationToken ct = default)
    {
        // Validate user level
        if (!await initiativeRepository.AuthorizedEntityModifyAsync(initiativeId, userName, false, ct))
        {
            return new(true)
            {
                StatusCode = HttpStatusCode.Forbidden,
            };
        }

        var query = entityRepository.GetQueryable();
        query = entityRepository.AddInitiativeFilter(initiativeId, query);

        return await GetOdataListByQueryAsync(query, queryOptions, ct);
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetByUserNameAsync(string userName, CancellationToken ct = default)
    {
        var dataListEntity = await entityRepository.GetByUserNameAsync(userName, ct);

        var dataListDto = dataListEntity
            .Select(mapper.Map);

        return new()
        {
            ResponseBody = dataListDto,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> AddAsync(JoinRequestDto entityData, CancellationToken ct = default)
    {
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
        var initiative = await initiativeRepository.GetByIdAsync(entityData.InitiativeId, ct);

        if (initiative == null)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Initiatives.NotFound),
            };
        }

        if (!initiative.Enabled)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Initiatives.Disabled),
            };
        }

        // Validate pending requests
        var hasPendingRequests = await entityRepository.AnyPendingRequests(initiative.Id, entityData.UserName, ct);

        if (hasPendingRequests)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.JoinRequests.PendingJoinRequests),
            };
        }

        // Validate user and initiative relationship
        var hasUserAndInitiativeRelationship = await initiativeUserRepository.IsDuplicatedAsync(initiative.Id, entityData.UserName, ct);

        if (hasUserAndInitiativeRelationship && entityData.Level != null)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.InitiativeUsers.Duplicated),
            };
        }

        if (!hasUserAndInitiativeRelationship && entityData.Level == null)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.JoinRequests.RelationshipDoesNotExists),
            };
        }

        // Get user data from external system
        var userData = await iamService.GetUserDataAsync(entityData.UserName, ct);

        if (userData == null)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Users.Invalid),
            };
        }

        // Build entity data
        entityData.Status = new EnumEntityDto<JoinRequestStatusEnum>(JoinRequestStatusEnum.UnderReview);
        entityData.CreationDate = DateTime.Now;
        var entity = mapper.Map(entityData);

        // Save data
        entity = await entityRepository.AddAsync(entity, ct);

        // Send notifications to initiative leaders
        var emailData = new Dictionary<string, object>()
        {
            { "InitiativeName", initiative.Name },
            { "UserName", userData.Username },
            { "JoinRequestStatus", JoinRequestStatusEnum.UnderReview },
            { "LeaveInitiative", entityData.Level == null },
        };

        await SendRequestNotificationToLeadersAsync(initiative.Id, emailData, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Create, "Added initiative join request", "{@EntityData}", entityData);

        return new()
        {
            ResponseBody = entityData,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> UpdateAsync(int id, JoinRequestDto entityData, CancellationToken ct = default)
    {
        // Validate user level
        var entity = await entityRepository.GetByIdAsync(id, ct);
        var initiativeId = entity?.InitiativeId ?? 0;

        if (!await initiativeRepository.AuthorizedEntityModifyAsync(initiativeId, entityData.ReviewerUserName, false, ct))
        {
            return new(true)
            {
                StatusCode = HttpStatusCode.Forbidden,
            };
        }

        // Validate initiative
        var initiative = await initiativeRepository.GetByIdAsync(initiativeId, ct);

        if (initiative == null)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Initiatives.NotFound),
            };
        }

        if (!initiative.Enabled)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Initiatives.Disabled),
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

        if (entity.StatusId is not (int)JoinRequestStatusEnum.UnderReview)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.JoinRequests.ReviewedJoinRequests),
            };
        }

        // Validate data
        var validationResult = await entityValidator.ValidateAsync(entityData, options => options.IncludeRuleSets("default", "Update"), ct);

        if (!validationResult.IsValid)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(validationResult.Errors),
            };
        }

        // Update entity data
        entity = await entityRepository.ReviewRequestAsync(id, entityData.ReviewerUserName, entityData.Status.Id, ct);

        if (entity == null)
        {
            return new(true)
            {
                StatusCode = HttpStatusCode.InternalServerError,
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.DatabaseError),
            };
        }

        entityData = mapper.Map(entity);

        // Send email
        var userData = await iamService.GetUserDataAsync(entityData.UserName, ct);

        var emailObject = new Dictionary<string, object>()
        {
            { "InitiativeName", initiative.Name },
            { "UserName", userData.Username },
            { "JoinRequestStatus", (JoinRequestStatusEnum)entity.StatusId },
            { "LeaveInitiative", entityData.Level == null },
        };

        await SendNotificationJoinRequestAsync(entityData.InitiativeId, userData, emailObject, ct);

        logger.AddLog(LogType.Update, "Updated initiative join request", "{@EntityData}", entityData);

        return new()
        {
            ResponseBody = entityData,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> CancelAsync(int id, string userName, CancellationToken ct = default)
    {
        // Validate entity
        var entity = await entityRepository.GetByIdAsync(id, ct);

        if (entity == null)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.ElementNotFound),
            };
        }

        // Validate user
        if (entity.UserName != userName || entity.StatusId != (int)JoinRequestStatusEnum.UnderReview)
        {
            return new(true)
            {
                StatusCode = HttpStatusCode.Forbidden,
            };
        }

        entity.StatusId = (int)JoinRequestStatusEnum.Cancelled;
        await entityRepository.UpdateAsync(entity, ct);

        var entityData = mapper.Map(entity);

        logger.AddLog(LogType.Delete, "Cancelled JoinRequest", "{@EntityData}", entityData);

        return new();
    }

    /// <inheritdoc/>
    public async Task SendNotificationsOldPendingRequestsAsync(CancellationToken ct = default)
    {
        const int oldPendingRequestsDays = 30;

        var pendingOldRequests = await entityRepository.GetPendingOldRequestsAsync(oldPendingRequestsDays, ct);

        if (pendingOldRequests?.Count > 0)
        {
            var leadersUserNames = pendingOldRequests
                .Select(e => e.Key)
                .ToArray();

            var leadersData = await iamService.GetUsersDataAsync(leadersUserNames, ct);

            var notificationsData = pendingOldRequests
                .Join(leadersData, por => por.Key, ld => ld.Username, (por, ld) => new { por, ld });

            var results = new List<bool>();
            var emailTasks = notificationsData.Select(async data =>
            {
                results.Add(await SendNotificationOldPendingRequestsAsync(data.ld, data.por.Value, ct));
            });

            await Task.WhenAll(emailTasks);
        }
    }

    /// <summary>
    /// Send join request notification.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="userData">External user data.</param>
    /// <param name="emailData">Email data.</param>
    /// <param name="ct">Cancellation token.</param>
    private async Task<bool> SendNotificationJoinRequestAsync(int initiativeId, ExternalUser userData, Dictionary<string, object> emailData, CancellationToken ct = default)
    {
        var notificationData = new SendNotificationData()
        {
            NotificationDto = new()
            {
                Properties = new()
                {
                    TemplateName = "JoinRequest",
                    Data = emailData,
                },
                Receiver = userData.Email,
            },
            Receivers = [new(userData.FullName, userData.Email)],
            SendToHiddenReceivers = true,
            InitiativeId = initiativeId,
        };

        await notificationService.SendNotificationAsync(notificationData, ct);

        return true;
    }

    /// <summary>
    /// Send notifications for old pending join requests.
    /// </summary>
    /// <param name="leaderData">Leader user data.</param>
    /// <param name="pendingRequests">Number of pending requests.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    private async Task<bool> SendNotificationOldPendingRequestsAsync(ExternalUser leaderData, int pendingRequests, CancellationToken ct = default)
    {
        var notificationData = new SendNotificationData()
        {
            NotificationDto = new()
            {
                Properties = new()
                {
                    TemplateName = "PendingRequestsReminder",
                    Data = new()
                    {
                        { "PendingRequestsCount", pendingRequests },
                    },
                },
                Receiver = leaderData.Email,
            },
            Receivers = [new(leaderData.FullName, leaderData.Email)],
            SendToHiddenReceivers = false,
        };

        await notificationService.SendNotificationAsync(notificationData, ct);

        return true;
    }

    /// <summary>
    /// Send new request notification to initiative leaders.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="emailData">Email data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    private async Task SendRequestNotificationToLeadersAsync(int initiativeId, Dictionary<string, object> emailData, CancellationToken ct = default)
    {
        var leaders = await initiativeUserRepository.GetByInitiativeAndLevelAsync(initiativeId, (int)InitiativeUserLevelEnum.Leader, ct);

        var leadersUserNames = leaders
            .Select(e => e.UserName)
            .ToArray();

        var leadersData = await iamService.GetUsersDataAsync(leadersUserNames, ct);

        var notificationsData = leaders
            .Join(leadersData, initiativeUser => initiativeUser.UserName, externalUser => externalUser.Username, (initiativeUser, externalUser) => new { initiativeUser, externalUser });

        var results = new List<bool>();
        var emailTasks = notificationsData.Select(async data =>
        {
            results.Add(await SendNotificationJoinRequestAsync(initiativeId, data.externalUser, emailData, ct));
        });

        await Task.WhenAll(emailTasks);
    }
}
