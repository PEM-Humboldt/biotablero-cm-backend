namespace IAVH.BioTablero.CM.Application.Services.Initiatives;

using System;
using System.Globalization;
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
using Microsoft.OData;

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
    private readonly IInitiativeUserRepository initiativeUserRepository;
    private readonly IWebViewTools webViewTools;
    private readonly IEmailService emailService;
    private readonly IIamService iamService;
    private readonly string iamSignInUrl;
    private readonly string frontEndInitiativeUrl;

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
        IInitiativeUserRepository initiativeUserRepository,
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
        this.webViewTools = webViewTools;
        this.emailService = emailService;
        this.iamService = iamService;
        iamSignInUrl = Environment.GetEnvironmentVariable("IAM_SIGN_IN_URL");
        frontEndInitiativeUrl = Environment.GetEnvironmentVariable("FRONTEND_INITIATIVE_URL");
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

        try
        {
            var odataResponse = await GetOdataDtoListByQueryAsync(query, queryOptions, ct);

            var userNames = odataResponse.DataList?
                .Select(e => e.Creator)
                .ToArray();

            var externalUsersData = await iamService.GetUsersDataAsync(userNames, ct);

            if (externalUsersData.Any())
            {
                foreach (var joinInvitationData in odataResponse.DataList)
                {
                    joinInvitationData.CreatorFullName = externalUsersData
                        .Where(i => i.Username == joinInvitationData.Creator)
                        .Select(i => i.FullName)
                        .FirstOrDefault();
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

        // Check if one or more users already belong to the initiative
        var emails = entityData.Guests
            .Select(e => e.Email)
            .ToArray();

        var anyUserBelongsToInitiative = await initiativeUserRepository.AnyByInitiativeAndUsersAsync(initiative.Id, emails, ct);

        if (anyUserBelongsToInitiative)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.JoinInvitations.ExistingUsers),
            };
        }

        // Check if an invitation already exists for one or more users
        var anyEmailsHaveBeenSent = await entityRepository.AnyAsync(initiative.Id, emails, ct);

        if (anyEmailsHaveBeenSent)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.JoinInvitations.ExistingInvitations),
            };
        }

        // Send invitation emails
        var result = await SendNotificationJoinInvitationAsync(emails, initiative, entityData, ct);

        if (result)
        {
            // Build entity data
            var entity = mapper.Map(entityData);
            entity.CreationDate = DateTimeOffset.UtcNow;

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
    /// <param name="joinInvitationData">Join Invitation data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if the process is successful. False otherwise.</returns>
    private async Task<bool> SendNotificationJoinInvitationAsync(string[] emails, Initiative initiative, JoinInvitationDto joinInvitationData, CancellationToken ct = default)
    {
        var receivers = emails
            .Select(e => new CustomEmailAddress(e))
            .ToArray();

        if (string.IsNullOrEmpty(joinInvitationData.CreatorFullName))
        {
            var userData = await iamService.GetUserDataAsync(joinInvitationData.Creator, ct);
            joinInvitationData.CreatorFullName = userData?.FullName;
        }

        var notificationDto = new NotificationDto()
        {
            Properties = new()
            {
                TemplateName = "JoinInvitation",
                Data = new()
                    {
                        { "InitiativeName", initiative.Name },
                        { "EmailMessage", joinInvitationData.Message },
                        { "SenderFullName", joinInvitationData.CreatorFullName },
                        { "Recipients", string.Join(',', emails) },
                        { "SignInUrl", iamSignInUrl },
                        { "InitiativeUrl", string.Format(CultureInfo.CurrentCulture, frontEndInitiativeUrl, initiative.Id) },
                    },
            },
        };

        joinInvitationData.HtmlMessage = await webViewTools.RenderViewToStringAsync(notificationDto.Properties.TemplateName, notificationDto);
        var response = await emailService.SendEmailAsync(notificationDto.Subject, receivers, null, joinInvitationData.HtmlMessage, ct);

        return !string.IsNullOrEmpty(response);
    }
}
