namespace IAVH.BioTablero.CM.Application.Services.Notifications;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.DTOs.Notifications;
using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Interfaces.Services.General;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Notifications;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Notifications;
using IAVH.BioTablero.CM.Core.Domain.Models.Email;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Notifications;

using Microsoft.AspNetCore.OData.Query;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

using InitiativeUserLevelEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.InitiativeUserLevel;

/// <summary>
/// Notification service.
/// </summary>
public class NotificationService : ServiceRead<Notification, NotificationDto, int>, INotificationService
{
    private readonly IValidator<NotificationDto> entityValidator;
    private readonly ILogger logger;
    private new readonly IMapperCreateAndRead<Notification, NotificationDto> mapper;
    private new readonly INotificationRepository entityRepository;
    private readonly IWebViewTools webViewTools;
    private readonly IEmailService emailService;
    private readonly IIamService iamService;
    private readonly IInitiativeUserRepository initiativeUserRepository;
    private readonly ISseNotificationDispatcher sseNotificationDispatcher;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="errorTranslator">Error translator.</param>
    /// <param name="entityValidator">Entity validator.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="webViewTools">Web View Tools.</param>
    /// <param name="emailService">Email service.</param>
    /// <param name="iamService">IAM service.</param>
    /// <param name="initiativeUserRepository">Initiative user repository.</param>
    /// <param name="sseNotificationDispatcher">SSE notification dispatcher.</param>
    public NotificationService(
        INotificationRepository entityRepository,
        IMapperCreateAndRead<Notification, NotificationDto> mapper,
        IValidationErrorTranslator errorTranslator,
        IValidator<NotificationDto> entityValidator,
        ILogger logger,
        IWebViewTools webViewTools,
        IEmailService emailService,
        IIamService iamService,
        IInitiativeUserRepository initiativeUserRepository,
        ISseNotificationDispatcher sseNotificationDispatcher)
        : base(entityRepository, mapper, errorTranslator)
    {
        this.entityRepository = entityRepository;
        this.mapper = mapper;
        this.entityValidator = entityValidator;
        this.logger = logger;
        this.webViewTools = webViewTools;
        this.emailService = emailService;
        this.iamService = iamService;
        this.initiativeUserRepository = initiativeUserRepository;
        this.sseNotificationDispatcher = sseNotificationDispatcher;
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetTotalNotReadByUserNameAsync(string userName, CancellationToken ct = default)
    {
        var total = await entityRepository.CountNotReadByUserNameAsync(userName, ct);

        return new()
        {
            ResponseBody = new Dictionary<string, int>
            {
                { "Total", total },
            },
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetItemAsync(int id, string userName, CancellationToken ct = default)
    {
        var entity = await entityRepository.GetByIdAsync(id, ct);

        if (entity != null)
        {
            if (entity.Receiver != userName)
            {
                return new(true)
                {
                    StatusCode = HttpStatusCode.Forbidden,
                };
            }

            if (!entity.IsRead)
            {
                entity.IsRead = true;

                if (entity.ReadingDate == null)
                {
                    entity.ReadingDate = DateTimeOffset.UtcNow;
                }

                await entityRepository.UpdateAsync(entity, ct);

                var entityDto = mapper.Map(entity);
                logger.AddLog(LogType.Update, "Read notification", "{@Entity}", entityDto);

                return new()
                {
                    ResponseBody = entityDto,
                };
            }
        }

        return new(true)
        {
            StatusCode = HttpStatusCode.NotFound,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetByUserNameAsync(string userName, ODataQueryOptions<Notification> queryOptions, CancellationToken ct = default)
    {
        var query = entityRepository.GetQueryable();
        query = entityRepository.GetQueryWithUserName(userName, query);
        return await GetOdataListByQueryAsync(query, queryOptions, ct);
    }

    /// <inheritdoc/>
    public async Task SendNotificationAsync(SendNotificationData notificationData, CancellationToken ct = default)
    {
        // Build HTML body
        var htmlBody = string.Empty;
        if (!string.IsNullOrEmpty(notificationData.NotificationDto.Properties.TemplateName))
        {
            htmlBody = await webViewTools.RenderViewToStringAsync(notificationData.NotificationDto.Properties.TemplateName, notificationData.NotificationDto);
        }

        // Validate data
        var validationResult = await entityValidator.ValidateAsync(notificationData.NotificationDto, ct);

        if (!validationResult.IsValid)
        {
            logger.AddLog(LogType.System, "Invalid notification values", "{@ValidationResults}", validationResult.Errors, Serilog.Events.LogEventLevel.Error);
            throw new ArgumentException("Invalid notification values", nameof(notificationData));
        }

        // Build entity data
        var entity = mapper.Map(notificationData.NotificationDto);
        entity.CreationDate = DateTimeOffset.UtcNow;

        // Save data
        entity = await entityRepository.AddAsync(entity, ct);
        notificationData.NotificationDto = mapper.Map(entity);

        logger.AddLog(LogType.Create, "Added notification", "{@EntityData}", notificationData.NotificationDto);

        // Send email
        if (notificationData.SendEmail && !string.IsNullOrEmpty(notificationData.NotificationDto.Properties.TemplateName))
        {
            CustomEmailAddress[] hiddenReceivers = null;

            if (notificationData.SendToHiddenReceivers && notificationData.InitiativeId != null)
            {
                hiddenReceivers = await GetHiddenReceiversAsync(notificationData.InitiativeId.Value, ct);
            }

            await emailService.SendEmailAsync(notificationData.NotificationDto.Subject, notificationData.Receivers, hiddenReceivers, htmlBody, ct);
        }

        // Dispatch SSE Notification
        await sseNotificationDispatcher.DispatchAsync(entity.Receiver, notificationData.NotificationDto);
    }

    /// <summary>
    /// Get hidden receivers for emails.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Hidden receivers list.</returns>
    private async Task<CustomEmailAddress[]> GetHiddenReceiversAsync(int initiativeId, CancellationToken ct = default)
    {
        var leaders = await initiativeUserRepository.GetByInitiativeAndLevelAsync(initiativeId, (int)InitiativeUserLevelEnum.Leader, ct);

        if (leaders?.Any() ?? false)
        {
            var leadersUserNames = leaders
                .Select(e => e.UserName)
                .ToArray();

            var leadersData = await iamService.GetUsersDataAsync(leadersUserNames, ct);

            return [.. leadersData.Select(e => new CustomEmailAddress(e.FullName, e.Email))];
        }

        return null;
    }
}
