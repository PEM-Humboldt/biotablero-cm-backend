namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.TerritoryStories;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.TerritoryStories;

using Microsoft.EntityFrameworkCore;

using Serilog;

/// <summary>
/// Territory Story Like repository.
/// </summary>
/// <param name="dbContext">General Database Context.</param>
/// <param name="logger">System logger.</param>
public class TerritoryStoryLikeRepository(
    GeneralContext dbContext,
    ILogger logger) : Repository<TerritoryStoryLike, int>(dbContext, logger), ITerritoryStoryLikeRepository
{
    /// <inheritdoc/>
    public async Task<bool> IsDuplicatedAsync(int territoryStoryId, string username, CancellationToken ct = default) =>
        await dbContext.TerritoryStoryLikes
            .Where(e => e.TerritoryStoryId == territoryStoryId && e.UserName == username)
            .AnyAsync(ct);

    /// <inheritdoc/>
    public async Task<TerritoryStoryLike?> GetByTerritoryStoryAndUserNameAsync(int territoryStoryId, string username, CancellationToken ct = default) =>
        await dbContext.TerritoryStoryLikes
            .Where(e => e.TerritoryStoryId == territoryStoryId && e.UserName == username)
            .FirstOrDefaultAsync(ct);
}
