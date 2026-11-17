namespace IAVH.BioTablero.CM.Application.Interfaces.Services.Notifications;

using System;
using System.Threading.Channels;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Notifications;

/// <summary>
/// SSE Notification Dispatcher interface.
/// </summary>
public interface ISseNotificationDispatcher
{
    /// <summary>
    /// Subscribes a user to SSE notifications.
    /// </summary>
    /// <param name="userName">The user's name.</param>
    /// <param name="connectionId">The connection identifier.</param>
    /// <returns>A channel reader for notifications.</returns>
    ChannelReader<NotificationDto> Subscribe(string userName, Guid connectionId);

    /// <summary>
    /// Unsubscribes a user from SSE notifications.
    /// </summary>
    /// <param name="userName">The user's name.</param>
    /// <param name="connectionId">The connection identifier.</param>
    void Unsubscribe(string userName, Guid connectionId);

    /// <summary>
    /// Dispatches a notification to all active connections for a user.
    /// </summary>
    /// <param name="userName">The user's name.</param>
    /// <param name="notificationDto">The notification data.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DispatchAsync(string userName, NotificationDto notificationDto);
}
