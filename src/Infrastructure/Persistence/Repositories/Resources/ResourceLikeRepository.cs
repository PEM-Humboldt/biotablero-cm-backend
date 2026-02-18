namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Resources;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Resources;

using Microsoft.EntityFrameworkCore;

using Serilog;

/// <summary>
/// Resource Like repository.
/// </summary>
public class ResourceLikeRepository : Repository<ResourceLike, int>, IResourceLikeRepository
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    /// <param name="logger">System logger.</param>
    public ResourceLikeRepository(
        GeneralContext dbContext,
        ILogger logger)
        : base(dbContext, logger)
    {
    }

    /// <inheritdoc/>
    public async Task<bool> IsDuplicatedAsync(int resourceId, string username, CancellationToken ct = default) =>
        await dbContext.ResourceLikes
            .Where(e => e.ResourceId == resourceId && e.UserName == username)
            .AnyAsync(ct);

    /// <inheritdoc/>
    public async Task<ResourceLike> GetByResourceAndUserNameAsync(int resourceId, string username, CancellationToken ct = default) =>
        await dbContext.ResourceLikes
            .Where(e => e.ResourceId == resourceId && e.UserName == username)
            .FirstOrDefaultAsync(ct);
}
