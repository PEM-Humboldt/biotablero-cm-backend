namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.Resources;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;

/// <summary>
/// Resource Like repository interface.
/// </summary>
public interface IResourceLikeRepository : IRepository<ResourceLike, int>
{
    /// <summary>
    /// Check if element is duplicated.
    /// </summary>
    /// <param name="resourceId">Resource identifier.</param>
    /// <param name="username">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    Task<bool> IsDuplicatedAsync(int resourceId, string username, CancellationToken ct = default);

    /// <summary>
    /// Finds an entity by territory story and user name.
    /// </summary>
    /// <param name="resourceId">Resource identifier.</param>
    /// <param name="username">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<ResourceLike> GetByResourceAndUserNameAsync(int resourceId, string username, CancellationToken ct = default);
}
