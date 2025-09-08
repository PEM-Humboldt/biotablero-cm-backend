namespace IAVH.BioTablero.CM.Application.Services.Initiatives;

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Specifications;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Utils.Email;
using IAVH.BioTablero.CM.Core.Domain.Utils.Iam;
using IAVH.BioTablero.CM.Core.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

using InitiativeUserLevelEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.InitiativeUserLevel;

/// <summary>
/// Initiative User service.
/// </summary>
public class InitiativeUserService : ServiceRead<InitiativeUser, InitiativeUserDto, int, InitiativeUserSpec>, IInitiativeUserService
{
    private const int MaxLeadersByInitiative = 3;
    private readonly IValidator<InitiativeUserDto> entityValidator;
    private readonly ILogger logger;
    private readonly IIamService iamService;
    private readonly IInitiativeRepository initiativeRepository;
    private readonly IWebViewTools webViewTools;
    private readonly IEmailService emailService;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="entityValidator">Entity validator.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="iamService">IAM service.</param>
    /// <param name="initiativeRepository">Initiative repository.</param>
    /// <param name="webViewTools">Web View Tools.</param>
    /// <param name="emailService">Email service.</param>
    public InitiativeUserService(
        IRepository<InitiativeUser> entityRepository,
        IMapper<InitiativeUser, InitiativeUserDto> mapper,
        IValidator<InitiativeUserDto> entityValidator,
        ILogger logger,
        IIamService iamService,
        IInitiativeRepository initiativeRepository,
        IWebViewTools webViewTools,
        IEmailService emailService)
        : base(entityRepository, mapper)
    {
        this.entityValidator = entityValidator;
        this.logger = logger;
        this.iamService = iamService;
        this.initiativeRepository = initiativeRepository;
        this.webViewTools = webViewTools;
        this.emailService = emailService;
    }

    /// <summary>
    /// Get entities by initiative.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> GetByInitiativeAsync(int initiativeId, CancellationToken ct = default)
    {
        var dataListEntity = await entityRepository.ListAsync(InitiativeUserSpec.InitiativeIdSpec(initiativeId), ct);

        var dataListDto = dataListEntity
            .Select(mapper.Map);

        return new()
        {
            ResponseBody = dataListDto,
        };
    }

    /// <summary>
    /// Add element.
    /// </summary>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> AddAsync(InitiativeUserDto entityData, CancellationToken ct = default)
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

        // Validate initiative
        var initiativeId = entityData.InitiativeId ?? 0;
        var initiativeExists = await initiativeRepository.AnyAsync(new InitiativeSpec(initiativeId), ct);

        if (!initiativeExists)
        {
            return new CustomWebResponse(true)
            {
                Message = "Initiative not found",
            };
        }

        // Validate duplicated entities
        var hasDuplicatedEntities = await entityRepository.AnyAsync(InitiativeUserSpec.UserNameSpec(initiativeId, entityData.UserName), ct);

        if (hasDuplicatedEntities)
        {
            return new CustomWebResponse(true)
            {
                Message = "The user already belongs to the initiative",
            };
        }

        // Validate number of leaders
        if (entityData.Level.Id == (int)InitiativeUserLevelEnum.Leader)
        {
            var totalUserLeaders = await entityRepository.CountAsync(InitiativeUserSpec.LevelSpec(initiativeId, (int)InitiativeUserLevelEnum.Leader), ct);

            if (totalUserLeaders >= MaxLeadersByInitiative)
            {
                return new CustomWebResponse(true)
                {
                    Message = $"Initiatives cannot have more than {MaxLeadersByInitiative} leaders",
                };
            }
        }

        // Validate user in external system
        var userExists = await iamService.UserExistsAsync(entityData.UserName, ct);

        if (!userExists)
        {
            return new CustomWebResponse(true)
            {
                Message = $"User is invalid or do not exist",
            };
        }

        // Build entity data
        var entity = mapper.Map(entityData);

        // Save data
        entity = await entityRepository.AddAsync(entity, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Create, "Added initiative user: {@entityData}", entityData);

        return new CustomWebResponse()
        {
            ResponseBody = entityData,
        };
    }

    /// <summary>
    /// Update element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="reviewerUserName">Reviewer user name.</param>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> UpdateAsync(int id, string reviewerUserName, InitiativeUserDto entityData, CancellationToken ct = default)
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
                Message = "Not found",
            };
        }

        // Validate number of leaders
        var leaders = await entityRepository.ListAsync(InitiativeUserSpec.LevelSpec(id, entity.InitiativeId, (int)InitiativeUserLevelEnum.Leader), ct);

        if (entity.LevelId == (int)InitiativeUserLevelEnum.Leader && entityData.Level.Id != (int)InitiativeUserLevelEnum.Leader)
        {
            if (leaders.Count is < 1)
            {
                return new CustomWebResponse(true)
                {
                    Message = $"At least one leader is required per initiative",
                };
            }
        }

        if (entityData.Level.Id == (int)InitiativeUserLevelEnum.Leader)
        {
            if (leaders.Count >= MaxLeadersByInitiative)
            {
                return new CustomWebResponse(true)
                {
                    Message = $"Initiatives cannot have more than {MaxLeadersByInitiative} leaders",
                };
            }
        }

        // Update entity data
        entity.LevelId = entityData.Level.Id;

        await entityRepository.UpdateAsync(entity, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Update, "Updated initiative user: {@entityData}", entityData);

        // Send email
        var userData = await iamService.GetUserDataAsync(entityData.UserName, ct);
        var initiative = await initiativeRepository.GetByIdAsync(entity.InitiativeId, ct);
        SendNotificationChangedLevel(userData, initiative, (InitiativeUserLevelEnum)entityData.Level.Id, reviewerUserName, leaders, ct);

        return new CustomWebResponse()
        {
            ResponseBody = entityData,
        };
    }

    /// <summary>
    /// Delete element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> DeleteAsync(int id, CancellationToken ct = default)
    {
        // Validate entity
        var entity = await entityRepository.GetByIdAsync(id, ct);

        if (entity == null)
        {
            return new CustomWebResponse(true)
            {
                Message = "Not found",
            };
        }

        // Validate number of leaders
        List<InitiativeUser> leaders = null;
        if (entity.LevelId == (int)InitiativeUserLevelEnum.Leader)
        {
            leaders = await entityRepository.ListAsync(InitiativeUserSpec.LevelSpec(entity.InitiativeId, (int)InitiativeUserLevelEnum.Leader), ct);

            if (leaders.Count is <= 1)
            {
                return new CustomWebResponse(true)
                {
                    Message = $"At least one leader is required per initiative",
                };
            }
        }

        await entityRepository.DeleteAsync(entity, ct);

        var entityData = mapper.Map(entity);

        logger.AddLog(LogType.Delete, "Deleted initiative user: {@entityData}", entityData);

        // Send email
        var userData = await iamService.GetUserDataAsync(entityData.UserName, ct);
        var initiative = await initiativeRepository.GetByIdAsync(entity.InitiativeId, ct);
        SendNotificationUserBanned(userData, initiative, leaders, ct);

        return new CustomWebResponse();
    }

    /// <summary>
    /// Send notification when a user's level has changed.
    /// </summary>
    /// <param name="userData">External user data.</param>
    /// <param name="initiative">Initiative data.</param>
    /// <param name="level">New user level.</param>
    /// <param name="reviewerUserName">Reviewer user name.</param>
    /// <param name="leaders">Initiative leaders list.</param>
    /// <param name="ct">Cancellation token.</param>
    private void SendNotificationChangedLevel(ExternalUser userData, Initiative initiative, InitiativeUserLevelEnum level, string reviewerUserName, List<InitiativeUser> leaders, CancellationToken ct = default) => Task.Run(
        async () =>
        {
            var newLevelName = string.Empty;

            newLevelName = level switch
            {
                InitiativeUserLevelEnum.Leader => "líder",
                InitiativeUserLevelEnum.Reader => "lector",
                _ => "miembro",
            };

            var emailData = new DefaultEmailData
            {
                Address = new(userData.FullName, userData.Email),
                Subject = string.Format(CultureInfo.InvariantCulture, "Ahora eres {0} de {1}", newLevelName, initiative.Name),
                Content = string.Format(CultureInfo.InvariantCulture, "Se te ha asignado el rol de {0} en la iniciativa '{1}' por el usuario '{2}'.", newLevelName, initiative.Name, reviewerUserName),
            };

            var receivers = new CustomEmailAddress[] { emailData.Address };
            CustomEmailAddress[] hiddenReceivers = null;
            var htmlBody = await webViewTools.RenderViewToStringAsync("Default", emailData);

            if (leaders?.Count > 0)
            {
                var leadersUserNames = leaders
                    .Select(e => e.UserName)
                    .ToArray();

                var leadersData = await iamService.GetUsersDataAsync(leadersUserNames, ct);
                hiddenReceivers = leadersData
                    .Select(u => new CustomEmailAddress(u.FullName, u.Email))
                    .ToArray();
            }

            await emailService.SendEmailAsync(emailData.Subject, receivers, hiddenReceivers, htmlBody, ct);
        },
        ct);

    /// <summary>
    /// Send notification when a user has been banned.
    /// </summary>
    /// <param name="userData">External user data.</param>
    /// <param name="initiative">Initiative data.</param>
    /// <param name="leaders">Initiative leaders list.</param>
    /// <param name="ct">Cancellation token.</param>
    private void SendNotificationUserBanned(ExternalUser userData, Initiative initiative, List<InitiativeUser> leaders, CancellationToken ct = default) => Task.Run(
        async () =>
        {
            var emailData = new DefaultEmailData
            {
                Address = new(userData.FullName, userData.Email),
                Subject = string.Format(CultureInfo.InvariantCulture, "Has sido retirado de {0}", initiative.Name),
                Content = string.Format(CultureInfo.InvariantCulture, "Tu membresía en la iniciativa '{0}' ha sido revocada. Si consideras que se trata de un error solicita unirte de nuevo.", initiative.Name),
            };

            var receivers = new CustomEmailAddress[] { emailData.Address };
            CustomEmailAddress[] hiddenReceivers = null;
            var htmlBody = await webViewTools.RenderViewToStringAsync("Default", emailData);

            if (leaders?.Count > 0)
            {
                var leadersUserNames = leaders
                    .Select(e => e.UserName)
                    .ToArray();

                var leadersData = await iamService.GetUsersDataAsync(leadersUserNames, ct);
                hiddenReceivers = leadersData
                    .Select(u => new CustomEmailAddress(u.FullName, u.Email))
                    .ToArray();
            }

            await emailService.SendEmailAsync(emailData.Subject, receivers, hiddenReceivers, htmlBody, ct);
        },
        ct);
}
