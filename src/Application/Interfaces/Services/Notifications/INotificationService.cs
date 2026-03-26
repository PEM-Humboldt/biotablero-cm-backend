namespace IAVH.BioTablero.CM.Application.Interfaces.Services.Notifications;

using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Notifications;

/// <summary>
/// Notification service interface.
/// </summary>
public interface INotificationService : IRead<Notification, int>
{
}
