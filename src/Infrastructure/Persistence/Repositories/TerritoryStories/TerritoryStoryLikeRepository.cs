namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.TerritoryStories;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.TerritoryStories;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Territory Story Like repository.
/// </summary>
public class TerritoryStoryLikeRepository : Repository<TerritoryStoryLike, int>, ITerritoryStoryLikeRepository
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    public TerritoryStoryLikeRepository(GeneralContext dbContext)
        : base(dbContext)
    {
    }

    /// <summary>
    /// Check if element is duplicated.
    /// </summary>
    /// <param name="territoryStoryId">Territory Story identifier.</param>
    /// <param name="username">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    public async Task<bool> IsDuplicatedAsync(int territoryStoryId, string username, CancellationToken ct = default) =>
        await dbContext.TerritoryStoryLikes
            .Where(e => e.TerritoryStoryId == territoryStoryId && e.UserName == username)
            .AnyAsync(ct);

    /// <summary>
    /// Finds an entity by territory story and user name.
    /// </summary>
    /// <param name="territoryStoryId">Territory Story identifier.</param>
    /// <param name="username">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<TerritoryStoryLike> GetByTerritoryStoryAndUserNameAsync(int territoryStoryId, string username, CancellationToken ct = default) =>
        await dbContext.TerritoryStoryLikes
            .Where(e => e.TerritoryStoryId == territoryStoryId && e.UserName == username)
            .FirstOrDefaultAsync(ct);
}
