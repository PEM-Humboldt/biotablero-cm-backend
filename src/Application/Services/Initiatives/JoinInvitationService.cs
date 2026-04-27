namespace IAVH.BioTablero.CM.Application.Services.Initiatives;

using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Notifications;
using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Interfaces.Services.General;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Initiatives;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Models.Email;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;

using Microsoft.AspNetCore.OData.Query;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

/// <summary>
/// Join Invitation service.
/// </summary>
public class JoinInvitationService : ServiceRead<JoinInvitation, JoinInvitationDto, int>, IJoinInvitationService
{
    private new readonly IJoinInvitationRepository entityRepository;
    private readonly IValidator<JoinInvitationDto> entityValidator;
    private readonly ILogger logger;
    private new readonly IMapperCreateAndRead<JoinInvitation, JoinInvitationDto> mapper;
    private readonly IInitiativeRepository initiativeRepository;
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
    /// <param name="webViewTools">Web View Tools.</param>
    /// <param name="emailService">Email service.</param>
    /// <param name="iamService">IAM service.</param>
    public JoinInvitationService(
        IJoinInvitationRepository entityRepository,
        IMapperCreateAndRead<JoinInvitation, JoinInvitationDto> mapper,
        IValidationErrorTranslator errorTranslator,
        IValidator<JoinInvitationDto> entityValidator,
        ILogger logger,
        IInitiativeRepository initiativeRepository,
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
        this.webViewTools = webViewTools;
        this.emailService = emailService;
        this.iamService = iamService;
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetListAsync(int initiativeId, string userName, ODataQueryOptions<JoinInvitation> queryOptions, CancellationToken ct = default)
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
        query = entityRepository.IncludeOdataEntities(query);

        return await GetOdataListByQueryAsync(query, queryOptions, ct);
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> AddAsync(JoinInvitationDto entityData, CancellationToken ct = default)
    {
        // Validate user permissions
        if (!await initiativeRepository.AuthorizedEntityModifyAsync(entityData.InitiativeId, entityData.Creator, false, ct))
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
        var initiative = await initiativeRepository.GetByIdAsync(entityData.InitiativeId, ct);

        if (initiative == null)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Initiatives.NotFound),
            };
        }

        // Validate duplicate emails
        bool hasDuplicateEmails = entityData.Guests
            .GroupBy(e => e.Email)
            .Any(g => g.Count() > 1);

        if (hasDuplicateEmails)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.JoinInvitations.DuplicatedEmails),
            };
        }

        // Validate emails in IAM service
        var emails = entityData.Guests
            .Select(e => e.Email)
            .ToArray();

        var externalUsersData = await iamService.GetUsersDataByEmailsAsync(emails, ct);

        if (externalUsersData.Any())
        {
            var existingEmails = externalUsersData
                .Select(e => e.Email);

            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.JoinInvitations.ExistingUsers, data: existingEmails),
            };
        }

        // Send invitation emails
        var result = await SendNotificationJoinInvitationAsync(emails, initiative, entityData.Message, ct);

        if (result)
        {
            // Build entity data
            var entity = mapper.Map(entityData);
            entity.CreationDate = DateTime.Now;

            // Save data
            entity = await entityRepository.AddAsync(entity, ct);

            entityData = mapper.Map(entity);

            logger.AddLog(LogType.Create, "Added initiative join invitation", "{@entityData}", entityData);

            return new()
            {
                ResponseBody = entityData,
            };
        }

        return new(true)
        {
            StatusCode = HttpStatusCode.InternalServerError,
            ResponseBody = errorTranslator.Translate(ValidationErrorCodes.JoinInvitations.EmailsSendingError),
        };
    }

    /// <summary>
    /// Send notification for join invitation.
    /// </summary>
    /// <param name="emails">Receivers emails.</param>
    /// <param name="initiative">Initiative data.</param>
    /// <param name="emailMessage">Email message.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if the process is successful. False otherwise.</returns>
    private async Task<bool> SendNotificationJoinInvitationAsync(string[] emails, Initiative initiative, string emailMessage, CancellationToken ct = default)
    {
        var receivers = emails
            .Select(e => new CustomEmailAddress(e))
            .ToArray();

        var notificationDto = new NotificationDto()
        {
            Properties = new()
            {
                TemplateName = "JoinInvitation",
                Data = new()
                    {
                        { "InitiativeName", initiative.Name },
                        { "EmailMessage", emailMessage },
                    },
            },
        };

        var htmlBody = await webViewTools.RenderViewToStringAsync(notificationDto.Properties.TemplateName, notificationDto);
        var response = await emailService.SendEmailAsync(notificationDto.Subject, receivers, null, htmlBody, ct);

        return !string.IsNullOrEmpty(response);
    }
}
