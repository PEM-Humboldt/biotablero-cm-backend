namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Initiative repository interface.
/// </summary>
public interface IInitiativeRepository : IRepository<Initiative, int>
{
    /// <summary>
    /// Include OData custom entities.
    /// </summary>
    /// <param name="query">Linq Query.</param>
    /// <returns>Modified Linq query.</returns>
    IQueryable<Initiative> IncludeOdataEntities(IQueryable<Initiative> query);

    /// <summary>
    /// Finds an entity with the given primary key value.
    /// </summary>
    /// <param name="id">The value of the primary key for the entity to be found.</param>
    /// <param name="userIsAuthenticated">User authenticated flag.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<Initiative> GetByIdAsync(int id, bool userIsAuthenticated, CancellationToken ct = default);

    /// <summary>
    /// Get elements by user name.
    /// </summary>
    /// <param name="userName">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Elements list.</returns>
    Task<IEnumerable<Initiative>> GetByUserNameAsync(string userName, CancellationToken ct = default);

    /// <summary>
    /// Check authorized entity modification.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="userIsAdmin">User administrator flag.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if the modification is authorized. False otherwise.</returns>
    Task<bool> AuthorizedEntityModifyAsync(int id, string userName, bool userIsAdmin, CancellationToken ct = default);

    /// <summary>
    /// Get if elements exists by name.
    /// </summary>
    /// <param name="name">Entity name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    Task<bool> AnyByNameAsync(string name, CancellationToken ct = default);

    /// <summary>
    /// Check if element is duplicated.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="name">Entity name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    Task<bool> IsDuplicatedAsync(int id, string name, CancellationToken ct = default);

    /// <summary>
    /// Check if elements exists by tag.
    /// </summary>
    /// <param name="tagId">Tag identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    Task<bool> AnyByTagAsync(int tagId, CancellationToken ct = default);

    /// <summary>
    /// Get active initiatives with coordinates by location.
    /// </summary>
    /// <param name="locationId">Location identifier (optional). If null, returns all active initiatives.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of initiatives with coordinates.</returns>
    Task<IEnumerable<Initiative>> GetActiveInitiativesWithCoordinatesByLocationAsync(int? locationId = null, CancellationToken ct = default);

    /// <summary>
    /// Get last created entities.
    /// </summary>
    /// <param name="count">Number of entities.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of last created initiatives.</returns>
    Task<IEnumerable<Initiative>> GetLastEntitiesAsync(int count, CancellationToken ct = default);
}
