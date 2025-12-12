namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.TerritoryStories;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;

/// <summary>
/// Territory Story Like repository interface.
/// </summary>
public interface ITerritoryStoryLikeRepository : IRepository<TerritoryStoryLike, int>
{
    /// <summary>
    /// Check if element is duplicated.
    /// </summary>
    /// <param name="territoryStoryId">Territory Story identifier.</param>
    /// <param name="username">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    Task<bool> IsDuplicatedAsync(int territoryStoryId, string username, CancellationToken ct = default);

    /// <summary>
    /// Finds an entity by territory story and user name.
    /// </summary>
    /// <param name="territoryStoryId">Territory Story identifier.</param>
    /// <param name="username">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<TerritoryStoryLike> GetByTerritoryStoryAndUserNameAsync(int territoryStoryId, string username, CancellationToken ct = default);
}
