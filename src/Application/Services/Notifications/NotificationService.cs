namespace IAVH.BioTablero.CM.Application.Services.Notifications;

using FluentValidation;

using IAVH.BioTablero.CM.Application.DTOs.Notifications;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Notifications;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Notifications;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Notifications;

using Serilog;

/// <summary>
/// Notification service.
/// </summary>
public class NotificationService : ServiceRead<Notification, NotificationDto, int>, INotificationService
{
    private readonly IValidator<NotificationDto> entityValidator;
    private readonly ILogger logger;
    private new readonly IMapperCreateAndRead<Notification, NotificationDto> mapper;
    private new readonly INotificationRepository entityRepository;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="errorTranslator">Error translator.</param>
    /// <param name="entityValidator">Entity validator.</param>
    /// <param name="logger">System logger.</param>
    public NotificationService(
        INotificationRepository entityRepository,
        IMapperCreateAndRead<Notification, NotificationDto> mapper,
        IValidationErrorTranslator errorTranslator,
        IValidator<NotificationDto> entityValidator,
        ILogger logger)
        : base(entityRepository, mapper, errorTranslator)
    {
        this.entityRepository = entityRepository;
        this.mapper = mapper;
        this.entityValidator = entityValidator;
        this.logger = logger;
    }
}
