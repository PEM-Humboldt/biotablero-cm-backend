namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Notifications;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Notifications;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Notifications;

using Microsoft.EntityFrameworkCore;

using Serilog;

/// <summary>
/// Notification repository.
/// </summary>
public class NotificationRepository : Repository<Notification, int>, INotificationRepository
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    /// <param name="logger">System logger.</param>
    public NotificationRepository(
        GeneralContext dbContext,
        ILogger logger)
        : base(dbContext, logger)
    {
    }

    /// <inheritdoc/>
    public IQueryable<Notification> GetQueryWithUserNameAsync(string userName, IQueryable<Notification> query, CancellationToken ct = default) =>
        query
            .Where(e => e.Receiver == userName);

    /// <inheritdoc/>
    public async Task<int> CountNotReadedByUserNameAsync(string userName, CancellationToken ct = default) =>
        await dbContext.Notifications
            .Where(e => e.Receiver == userName && !e.Readed)
            .CountAsync(ct);
}
