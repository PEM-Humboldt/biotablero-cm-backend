namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.Resources;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;

/// <summary>
/// Resource repository interface.
/// </summary>
public interface IResourceRepository : IRepository<Resource, int>
{
    /// <summary>
    /// Get query with initiative and user name filters.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="query">Linq Query.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Custom query.</returns>
    Task<IQueryable<Resource>> GetQueryWithInitiativeAndUserNameAsync(int initiativeId, string userName, IQueryable<Resource> query, CancellationToken ct = default);

    /// <summary>
    /// Check resource and user relationship (by initiative).
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if the relationship exists. False otherwise.</returns>
    Task<bool> UserRelationshipExistsAsync(int id, string userName, CancellationToken ct = default);

    /// <summary>
    /// Check if element is duplicated.
    /// </summary>
    /// <param name="name">Entity name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    Task<bool> IsDuplicatedAsync(string name, CancellationToken ct = default);

    /// <summary>
    /// Check if element is duplicated.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="name">Entity name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    Task<bool> IsDuplicatedAsync(int id, string name, CancellationToken ct = default);

    /// <summary>
    /// Updates an entity in the database.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="userName">The editor user name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<Resource> UpdateAsync(Resource entity, string userName, CancellationToken ct = default);
}
