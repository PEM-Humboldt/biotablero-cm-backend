namespace IAVH.BioTablero.CM.Application.Services.Initiatives;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Utils;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Specifications;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;
using IAVH.BioTablero.CM.Core.Domain.Utils.Email;
using IAVH.BioTablero.CM.Core.Domain.Utils.Iam;
using IAVH.BioTablero.CM.Core.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using Microsoft.AspNetCore.OData.Query;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

using InitiativeUserLevelEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.InitiativeUserLevel;
using JoinRequestStatusEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.JoinRequestStatus;

/// <summary>
/// Join Request service.
/// </summary>
public class JoinRequestService : ServiceRead<JoinRequest, JoinRequestDto, int, JoinRequestSpec>, IJoinRequestService
{
    private new readonly IJoinRequestRepository entityRepository;
    private readonly ILogger logger;
    private readonly IInitiativeRepository initiativeRepository;
    private readonly IRepository<InitiativeUser> initiativeUserRepository;
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
    /// <param name="initiativeUserRepository">Initiative user repository.</param>
    /// <param name="webViewTools">Web View Tools.</param>
    /// <param name="emailService">Email service.</param>
    /// <param name="iamService">IAM service.</param>
    public JoinRequestService(
        IJoinRequestRepository entityRepository,
        IMapper<JoinRequest, JoinRequestDto> mapper,
        ILogger logger,
        IInitiativeRepository initiativeRepository,
        IRepository<InitiativeUser> initiativeUserRepository,
        IWebViewTools webViewTools,
        IEmailService emailService,
        IIamService iamService)
        : base(entityRepository, mapper)
    {
        this.entityRepository = entityRepository;
        this.logger = logger;
        this.initiativeRepository = initiativeRepository;
        this.initiativeUserRepository = initiativeUserRepository;
        this.webViewTools = webViewTools;
        this.emailService = emailService;
        this.iamService = iamService;
    }

    /// <summary>
    /// Get elements list (OData).
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="queryOptions">OData query options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> GetListAsync(int initiativeId, string userName, ODataQueryOptions<JoinRequest> queryOptions, CancellationToken ct = default)
    {
        // Validate user level
        var userIsLeader = await initiativeUserRepository.AnyAsync(InitiativeUserSpec.UserLevelSpec(initiativeId, userName, (int)InitiativeUserLevelEnum.Leader), ct);

        if (!userIsLeader)
        {
            return new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.Forbidden,
            };
        }

        var query = entityRepository.GetQueryable();
        query = entityRepository.AddInitiativeFilter(initiativeId, query);

        return await GetOdataListByQueryAsync(query, queryOptions, ct);
    }

    /// <summary>
    /// Add element.
    /// </summary>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> AddAsync(JoinRequestDto entityData, CancellationToken ct = default)
    {
        // Validate initiative
        var initiative = await initiativeRepository.GetByIdAsync(entityData.InitiativeId, ct);

        if (initiative == null)
        {
            return new CustomWebResponse(true)
            {
                Message = "Initiative not found",
            };
        }

        // Validate pending requests
        var hasPendingRequests = await entityRepository.AnyAsync(JoinRequestSpec.PendingRequests(entityData.InitiativeId, entityData.UserName), ct);

        if (hasPendingRequests)
        {
            return new CustomWebResponse(true)
            {
                Message = "There are one or more pending join requests",
            };
        }

        // Validate user and initiative relationship
        var hasUserAndInitiativeRelationship = await initiativeUserRepository.AnyAsync(InitiativeUserSpec.UserNameSpec(entityData.InitiativeId, entityData.UserName), ct);

        if (hasUserAndInitiativeRelationship)
        {
            return new CustomWebResponse(true)
            {
                Message = "The user already belongs to the initiative",
            };
        }

        // Get user data from external system
        var userData = await iamService.GetUserDataAsync(entityData.UserName, ct);

        if (userData == null)
        {
            return new CustomWebResponse(true)
            {
                Message = "User is invalid or do not exist",
            };
        }

        // Build entity data
        entityData.Status = new EnumEntityDto<JoinRequestStatusEnum>(JoinRequestStatusEnum.UnderReview);
        var entity = mapper.Map(entityData);
        entity.CreationDate = DateTime.Now;

        // Save data
        entity = await entityRepository.AddAsync(entity, ct);

        // Send email
        var emailObject = new DefaultEmailData()
        {
            Address = new(userData.FullName, userData.Email),
            Subject = string.Format(CultureInfo.InvariantCulture, "Solicitud de ingreso a '{0}'", initiative.Name),
            Content = string.Format(CultureInfo.InvariantCulture, "El usuario '{0}' ha realizado una solicitud de acceso para la iniciativa '{1}'", userData.Username, initiative.Name),
        };

        SendNotificationJoinRequest(entityData.InitiativeId, emailObject, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Create, "Added initiative join request: {@EntityData}", entityData);

        return new CustomWebResponse()
        {
            ResponseBody = entityData,
        };
    }

    /// <summary>
    /// Update element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> UpdateAsync(int id, JoinRequestDto entityData, CancellationToken ct = default)
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

        // Validate user level
        var userIsLeader = await initiativeUserRepository.AnyAsync(InitiativeUserSpec.UserLevelSpec(entity.InitiativeId, entityData.ReviewerUserName, (int)InitiativeUserLevelEnum.Leader), ct);

        if (!userIsLeader)
        {
            return new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.Forbidden,
            };
        }

        if (entityData.Status.Id == (int)JoinRequestStatusEnum.UnderReview)
        {
            return new CustomWebResponse(true)
            {
                Message = "Invalid selected status",
            };
        }

        if (entity.StatusId != (int)JoinRequestStatusEnum.UnderReview)
        {
            return new CustomWebResponse(true)
            {
                Message = "The join request has already been reviewed",
            };
        }

        // Update entity data
        entity = await entityRepository.ReviewRequestAsync(id, entityData.ReviewerUserName, entity.UserName, entityData.Status.Id, ct);

        if (entity == null)
        {
            return new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Message = "Database error",
            };
        }

        entityData = mapper.Map(entity);

        // Send email
        var userData = await iamService.GetUserDataAsync(entityData.UserName, ct);
        var initiative = await initiativeRepository.GetByIdAsync(entityData.InitiativeId, ct);

        var emailObject = new DefaultEmailData()
        {
            Address = new(userData.FullName, userData.Email),
        };

        if (entity.StatusId == (int)JoinRequestStatusEnum.Approved)
        {
            emailObject.Subject = string.Format(CultureInfo.InvariantCulture, "Solicitud de ingreso a '{0}' aprobada", initiative.Name);
            emailObject.Content = string.Format(CultureInfo.InvariantCulture, "Bienvenido a '{0}'.<br /> Ya puedes aportar al estado de la biodiversidad de nuestro territorio.", initiative.Name);
        }
        else
        {
            emailObject.Subject = string.Format(CultureInfo.InvariantCulture, "Solicitud de ingreso a '{0}' rechazada", initiative.Name);
            emailObject.Content = "Tu solicitud ha sido rechazada. Si consideras que se trata de un error solicita de nuevo.";
        }

        SendNotificationJoinRequest(entityData.InitiativeId, emailObject, ct);

        logger.AddLog(LogType.Update, "Updated initiative join request: {@EntityData}", entityData);

        return new CustomWebResponse()
        {
            ResponseBody = entityData,
        };
    }

    /// <summary>
    /// Send notifications for old pending join requests.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
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
    /// <param name="emailData">Email data.</param>
    /// <param name="ct">Cancellation token.</param>
    private void SendNotificationJoinRequest(int initiativeId, DefaultEmailData emailData, CancellationToken ct = default) => _ = Task.Run(
        async () =>
        {
            var leaders = await initiativeUserRepository.ListAsync(InitiativeUserSpec.LevelSpec(initiativeId, (int)InitiativeUserLevelEnum.Leader), ct);

            if (leaders?.Count > 0)
            {
                var leadersUserNames = leaders
                    .Select(e => e.UserName)
                    .ToArray();

                var leadersData = await iamService.GetUsersDataAsync(leadersUserNames, ct);

                var hiddenReceivers = leadersData
                    .Select(e => new CustomEmailAddress(e.FullName, e.Email))
                    .ToArray();

                var receivers = new CustomEmailAddress[] { emailData.Address };

                var emailObject = new DefaultEmailData()
                {
                    Content = emailData.Content,
                };
                var htmlBody = await webViewTools.RenderViewToStringAsync("Default", emailObject);

                await emailService.SendEmailAsync(emailData.Subject, receivers, hiddenReceivers, htmlBody, ct);
            }
        },
        ct);

    /// <summary>
    /// Send notifications for old pending join requests.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    private async Task<bool> SendNotificationOldPendingRequestsAsync(ExternalUser leaderData, int pendingRequests, CancellationToken ct = default)
    {
        var emailData = new DefaultEmailData
        {
            Address = new(leaderData.FullName, leaderData.Email),
            Subject = "Tienes solicitudes de ingreso pendientes de revisión",
            Content = string.Format(CultureInfo.InvariantCulture, "Tienes <b>{0}</b> solicitudes de ingreso que necesitan tu revisión.", pendingRequests),
        };

        var receivers = new CustomEmailAddress[] { emailData.Address };
        var htmlBody = await webViewTools.RenderViewToStringAsync("Default", emailData);

        await emailService.SendEmailAsync(emailData.Subject, receivers, null, htmlBody, ct);

        return true;
    }
}
