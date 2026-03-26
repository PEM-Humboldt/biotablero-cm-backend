namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.Notifications;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Notifications;

/// <summary>
/// Notification repository interface.
/// </summary>
public interface INotificationRepository : IRepository<Notification, int>
{
    /// <summary>
    /// Get query with user name filter.
    /// </summary>
    /// <param name="userName">User name.</param>
    /// <param name="query">Linq Query.</param>
    /// <returns>Custom query.</returns>
    IQueryable<Notification> GetQueryWithUserName(string userName, IQueryable<Notification> query);

    /// <summary>
    /// Count not readed elements by user name.
    /// </summary>
    /// <param name="userName">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Not readed total elements.</returns>
    Task<int> CountNotReadedByUserNameAsync(string userName, CancellationToken ct = default);
}
