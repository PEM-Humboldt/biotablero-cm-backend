namespace IAVH.BioTablero.CM.Application.Interfaces.Services.Notifications;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.DTOs.Notifications;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Notifications;

using Microsoft.AspNetCore.OData.Query;

/// <summary>
/// Notification service interface.
/// </summary>
public interface INotificationService : IRead<Notification, int>
{
    /// <summary>
    /// Get total unreaded entities by user name.
    /// </summary>
    /// <param name="userName">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> GetTotalUnreadedByUserNameAsync(string userName, CancellationToken ct = default);

    /// <summary>
    /// Get element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> GetItemAsync(int id, string userName, CancellationToken ct = default);

    /// <summary>
    /// Get entities by user name (paginated).
    /// </summary>
    /// <param name="userName">User name.</param>
    /// <param name="queryOptions">OData query options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> GetByUserNameAsync(string userName, ODataQueryOptions<Notification> queryOptions, CancellationToken ct = default);

    /// <summary>
    /// Send notification.
    /// </summary>
    /// <param name="entityData">Entity data.</param>
    /// <param name="sendEmail">Send email flag.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task SendNotificationAsync(NotificationDto entityData, bool sendEmail, CancellationToken ct = default);
}
