namespace IAVH.BioTablero.CM.Application.Services.Initiatives;

using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Specifications;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Utils.Email;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using Microsoft.AspNetCore.OData.Query;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

using InitiativeUserLevelEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.InitiativeUserLevel;

/// <summary>
/// Join Invitation service.
/// </summary>
public class JoinInvitationService : ServiceRead<JoinInvitation, JoinInvitationDto, int, JoinInvitationSpec>, IJoinInvitationService
{
    private new readonly IJoinInvitationRepository entityRepository;
    private readonly IValidator<JoinInvitationDto> entityValidator;
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
    public JoinInvitationService(
        IJoinInvitationRepository entityRepository,
        IMapper<JoinInvitation, JoinInvitationDto> mapper,
        IValidator<JoinInvitationDto> entityValidator,
        ILogger logger,
        IInitiativeRepository initiativeRepository,
        IRepository<InitiativeUser> initiativeUserRepository,
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
    public async Task<CustomWebResponse> GetListAsync(int initiativeId, string userName, ODataQueryOptions<JoinInvitation> queryOptions, CancellationToken ct = default)
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
        query = entityRepository.IncludeOdataEntities(query);

        return await GetOdataListByQueryAsync(query, queryOptions, ct);
    }

    /// <summary>
    /// Add element.
    /// </summary>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> AddAsync(JoinInvitationDto entityData, CancellationToken ct = default)
    {
        // Validate user role and initiative relationship
        var userIsLeader = await initiativeUserRepository.AnyAsync(InitiativeUserSpec.UserLevelSpec(entityData.InitiativeId, entityData.Creator, (int)InitiativeUserLevelEnum.Leader), ct);

        if (!userIsLeader)
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
        var initiative = await initiativeRepository.GetByIdAsync(entityData.InitiativeId, ct);

        if (initiative == null)
        {
            return new CustomWebResponse(true)
            {
                Message = "Initiative not found",
            };
        }

        // Validate duplicate emails
        bool hasDuplicateEmails = entityData.Guests
            .GroupBy(e => e.Email)
            .Any(g => g.Count() > 1);

        if (hasDuplicateEmails)
        {
            return new CustomWebResponse(true)
            {
                Message = "There are duplicate emails",
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

            return new CustomWebResponse(true)
            {
                Message = "One or more users with the entered email addresses are already in the system",
                ResponseBody = existingEmails,
            };
        }

        // Send invitation emails
        var result = await SendNotificationJoinInvitation(emails, initiative, entityData.Message, ct);

        if (result)
        {
            // Build entity data
            var entity = mapper.Map(entityData);
            entity.CreationDate = DateTime.Now;

            // Save data
            entity = await entityRepository.AddAsync(entity, ct);

            entityData = mapper.Map(entity);

            logger.AddLog(LogType.Create, "Added initiative join invitation: {@entityData}", entityData);

            return new CustomWebResponse()
            {
                ResponseBody = entityData,
            };
        }

        return new CustomWebResponse(true)
        {
            Message = "Failed emails sending.",
            StatusCode = HttpStatusCode.InternalServerError,
        };
    }

    private async Task<bool> SendNotificationJoinInvitation(string[] emails, Initiative initiative, string emailMessage, CancellationToken ct = default)
    {
        var emailData = new DefaultEmailData
        {
            Subject = string.Format(CultureInfo.InvariantCulture, "Invitación a unirse a {0}", initiative.Name),
            Content = string.Format(CultureInfo.InvariantCulture, "Has sido invitado a unirte a la iniciativa '{0}'.<br /> {1}.", initiative.Name, emailMessage),
        };

        var receivers = emails
            .Select(e => new CustomEmailAddress(e))
            .ToArray();

        var htmlBody = await webViewTools.RenderViewToStringAsync("JoinInvitation", emailData);

        var response = await emailService.SendEmailAsync(emailData.Subject, receivers, null, htmlBody, ct);

        return !string.IsNullOrEmpty(response);
    }
}
