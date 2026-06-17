namespace IAVH.BioTablero.CM.Application.Services.Initiatives;

using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
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

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.LogEnums;

using InitiativeUserLevelEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.InitiativeUserLevel;

/// <summary>
/// Initiative User service.
/// </summary>
public class InitiativeUserService : ServiceRead<InitiativeUser, InitiativeUserDto, int>, IInitiativeUserService
{
    private const int MaxLeadersByInitiative = 3;
    private new readonly IInitiativeUserRepository entityRepository;
    private readonly IValidator<InitiativeUserDto> entityValidator;
    private readonly ILogger logger;
    private new readonly IMapperCreateReadAndUpdate<InitiativeUser, InitiativeUserDto> mapper;
    private readonly IIamService iamService;
    private readonly IInitiativeRepository initiativeRepository;
    private readonly INotificationService notificationService;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="errorTranslator">Error translator.</param>
    /// <param name="entityValidator">Entity validator.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="iamService">IAM service.</param>
    /// <param name="initiativeRepository">Initiative repository.</param>
    /// <param name="notificationService">Notification service.</param>
    public InitiativeUserService(
        IInitiativeUserRepository entityRepository,
        IMapperCreateReadAndUpdate<InitiativeUser, InitiativeUserDto> mapper,
        IValidationErrorTranslator errorTranslator,
        IValidator<InitiativeUserDto> entityValidator,
        ILogger logger,
        IIamService iamService,
        IInitiativeRepository initiativeRepository,
        INotificationService notificationService)
        : base(entityRepository, mapper, errorTranslator)
    {
        this.entityRepository = entityRepository;
        this.mapper = mapper;
        this.entityValidator = entityValidator;
        this.logger = logger;
        this.iamService = iamService;
        this.initiativeRepository = initiativeRepository;
        this.notificationService = notificationService;
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetByInitiativeAsync(int initiativeId, CancellationToken ct = default)
    {
        var dataListEntity = await entityRepository.GetByInitiativeAsync(initiativeId, ct);

        var dataListDto = dataListEntity
            .Select(mapper.Map);

        return new()
        {
            ResponseBody = dataListDto,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> AddAsync(InitiativeUserDto entityData, CancellationToken ct = default)
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
        var initiativeId = entityData.InitiativeId ?? 0;
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

        // Validate duplicated entities
        var hasDuplicatedEntities = await entityRepository.IsDuplicatedAsync(initiativeId, entityData.UserName, ct);

        if (hasDuplicatedEntities)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.InitiativeUsers.Duplicated),
            };
        }

        // Validate number of leaders
        if (entityData.Level.Id == (int)InitiativeUserLevelEnum.Leader)
        {
            var leaders = await entityRepository.GetByInitiativeAndLevelAsync(initiativeId, (int)InitiativeUserLevelEnum.Leader, ct);

            if (leaders.Count() >= MaxLeadersByInitiative)
            {
                return new(true)
                {
                    ResponseBody = errorTranslator.Translate(ValidationErrorCodes.InitiativeUsers.LeaderLimitExceeded, data: MaxLeadersByInitiative),
                };
            }
        }

        // Validate user in external system
        var userExists = await iamService.UserExistsAsync(entityData.UserName, ct);

        if (!userExists)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Users.Invalid),
            };
        }

        // Build entity data
        var entity = mapper.Map(entityData);

        // Save data
        entity = await entityRepository.AddAsync(entity, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Create, "Added initiative user", "{@EntityData}", entityData);

        return new()
        {
            ResponseBody = entityData,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> UpdateAsync(int id, string reviewerUserName, bool userIsAdmin, InitiativeUserDto entityData, CancellationToken ct = default)
    {
        // Validate user permissions
        var entity = await entityRepository.GetByIdAsync(id, ct);
        var initiativeId = entity?.InitiativeId ?? 0;

        if (!await initiativeRepository.AuthorizedEntityModifyAsync(initiativeId, reviewerUserName, userIsAdmin, ct))
        {
            return new(true)
            {
                StatusCode = HttpStatusCode.Forbidden,
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

        // Validate initiative
        var initiative = await initiativeRepository.GetByIdAsync(initiativeId, ct);

        if (!initiative.Enabled)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Initiatives.Disabled),
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

        // Validate number of leaders
        var leaders = await entityRepository.GetByInitiativeAndLevelAsync(id, entity.InitiativeId, (int)InitiativeUserLevelEnum.Leader, ct);

        if (entity.LevelId == (int)InitiativeUserLevelEnum.Leader && entityData.Level.Id != (int)InitiativeUserLevelEnum.Leader && leaders.Count() <= 1)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.InitiativeUsers.LeadersRequired),
            };
        }

        if (entityData.Level.Id == (int)InitiativeUserLevelEnum.Leader && leaders.Count() >= MaxLeadersByInitiative)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.InitiativeUsers.LeaderLimitExceeded, data: MaxLeadersByInitiative),
            };
        }

        // Update entity data
        mapper.Update(entity, entityData);

        await entityRepository.UpdateAsync(entity, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Update, "Updated initiative user", "{@EntityData}", entityData);

        // Send email
        var userData = await iamService.GetUserDataAsync(entityData.UserName, ct);
        await SendNotificationChangedLevelAsync(userData, initiative, (InitiativeUserLevelEnum)entityData.Level.Id, reviewerUserName, ct);

        return new()
        {
            ResponseBody = entityData,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> DeleteAsync(int id, string userName, bool userIsAdmin, CancellationToken ct = default)
    {
        // Validate user permissions
        var entity = await entityRepository.GetByIdAsync(id, ct);
        var initiativeId = entity?.InitiativeId ?? 0;
        var userIsAdminOrLeader = await initiativeRepository.AuthorizedEntityModifyAsync(initiativeId, userName, userIsAdmin, ct);

        if (!userIsAdminOrLeader && entity?.UserName != userName)
        {
            return new(true)
            {
                StatusCode = HttpStatusCode.Forbidden,
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

        // Validate initiative
        var initiative = await initiativeRepository.GetByIdAsync(initiativeId, ct);

        if (!initiative.Enabled)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.Initiatives.Disabled),
            };
        }

        if (entity.LevelId == (int)InitiativeUserLevelEnum.Leader)
        {
            // Validate number of leaders
            var leaders = await entityRepository.GetByInitiativeAndLevelAsync(entity.InitiativeId, (int)InitiativeUserLevelEnum.Leader, ct);

            if (leaders.Count() is <= 1)
            {
                return new(true)
                {
                    ResponseBody = errorTranslator.Translate(ValidationErrorCodes.InitiativeUsers.LeadersRequired),
                };
            }
        }

        await entityRepository.DeleteAsync(entity, ct);

        var entityData = mapper.Map(entity);

        logger.AddLog(LogType.Delete, "Deleted initiative user", "{@EntityData}", entityData);

        // Send email
        var userData = await iamService.GetUserDataAsync(entityData.UserName, ct);
        await SendNotificationUserBannedAsync(userData, initiative, ct);

        return new();
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> UpdateFocusAreaAsync(int initiativeId, string userName, InitiativeUserDto entityData, CancellationToken ct = default)
    {
        var entity = await entityRepository.GetByInitiativeAndUserNameAsync(initiativeId, userName, ct);

        if (entity == null)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.ElementNotFound),
            };
        }

        entity.FocusArea = entityData.FocusArea;

        await entityRepository.UpdateAsync(entity, ct);

        entityData = mapper.Map(entity);

        logger.AddLog(LogType.Update, "Updated initiative user focus area", "{@EntityData}", entityData);

        return new()
        {
            ResponseBody = entityData,
        };
    }

    /// <summary>
    /// Send notification when a user's level has changed.
    /// </summary>
    /// <param name="userData">External user data.</param>
    /// <param name="initiative">Initiative data.</param>
    /// <param name="level">New user level.</param>
    /// <param name="reviewerUserName">Reviewer user name.</param>
    /// <param name="ct">Cancellation token.</param>
    private async Task SendNotificationChangedLevelAsync(ExternalUser userData, Initiative initiative, InitiativeUserLevelEnum level, string reviewerUserName, CancellationToken ct = default)
    {
        var newLevelName = level switch
        {
            InitiativeUserLevelEnum.Leader => "líder",
            InitiativeUserLevelEnum.Reader => "lector",
            InitiativeUserLevelEnum.Collaborator => "colaborador",
            _ => "N/A",
        };

        var notificationData = new SendNotificationData()
        {
            NotificationDto = new()
            {
                Properties = new()
                {
                    TemplateName = "RoleAssignment",
                    Data = new()
                    {
                        { "InitiativeName", initiative.Name },
                        { "LevelName", newLevelName },
                        { "ReviewerUserName", reviewerUserName },
                    },
                },
                Receiver = userData.Email,
            },
            Receivers = [new(userData.FullName, userData.Email)],
            SendToHiddenReceivers = true,
            InitiativeId = initiative.Id,
        };

        await notificationService.SendNotificationAsync(notificationData, ct);
    }

    /// <summary>
    /// Send notification when a user has been banned.
    /// </summary>
    /// <param name="userData">External user data.</param>
    /// <param name="initiative">Initiative data.</param>
    /// <param name="ct">Cancellation token.</param>
    private async Task SendNotificationUserBannedAsync(ExternalUser userData, Initiative initiative, CancellationToken ct = default)
    {
        var notificationData = new SendNotificationData()
        {
            NotificationDto = new()
            {
                Properties = new()
                {
                    TemplateName = "UserRemoval",
                    Data = new()
                    {
                        { "InitiativeName", initiative.Name },
                    },
                },
                Receiver = userData.Email,
            },
            Receivers = [new(userData.FullName, userData.Email)],
            SendToHiddenReceivers = true,
            InitiativeId = initiative.Id,
        };

        await notificationService.SendNotificationAsync(notificationData, ct);
    }
}
