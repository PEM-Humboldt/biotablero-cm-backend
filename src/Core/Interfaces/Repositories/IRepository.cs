namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Base repository interface.
/// </summary>
/// <typeparam name="TE">Entity class type.</typeparam>
/// <typeparam name="TI">Entity identifier type.</typeparam>
public interface IRepository<TE, TI> : IDisposable
    where TE : class, IAggregateRoot
    where TI : notnull
{
    /// <summary>
    /// Finds an entity with the given primary key value.
    /// </summary>
    /// <param name="id">The value of the primary key for the entity to be found.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<TE> GetByIdAsync(TI id, CancellationToken ct = default);

    /// <summary>
    /// Finds all entities of <typeparamref name="TE" /> from the database.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<List<TE>> ListAsync(CancellationToken ct = default);

    /// <summary>
    /// Returns the total number of records.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<int> CountAsync(CancellationToken ct = default);

    /// <summary>
    /// Returns a boolean whether any entity exists or not.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<bool> AnyAsync(CancellationToken ct = default);

    /// <summary>
    /// Adds an entity in the database.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<TE> AddAsync(TE entity, CancellationToken ct = default);

    /// <summary>
    /// Adds the given entities in the database.
    /// </summary>
    /// <param name="entities">The entities to add.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<IEnumerable<TE>> AddRangeAsync(IEnumerable<TE> entities, CancellationToken ct = default);

    /// <summary>
    /// Updates an entity in the database.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<int> UpdateAsync(TE entity, CancellationToken ct = default);

    /// <summary>
    /// Updates the given entities in the database.
    /// </summary>
    /// <param name="entities">The entities to update.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<int> UpdateRangeAsync(IEnumerable<TE> entities, CancellationToken ct = default);

    /// <summary>
    /// Removes an entity in the database.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<int> DeleteAsync(TE entity, CancellationToken ct = default);

    /// <summary>
    /// Removes the given entities in the database.
    /// </summary>
    /// <param name="entities">The entities to remove.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<int> DeleteRangeAsync(IEnumerable<TE> entities, CancellationToken ct = default);

    /// <summary>
    /// Persists changes to the database.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<int> SaveChangesAsync(CancellationToken ct = default);

    /// <summary>
    /// Get a new query.
    /// </summary>
    /// <returns>Linq Query.</returns>
    IQueryable<TE> GetQueryable();

    /// <summary>
    /// Get the number of elements in a query.
    /// </summary>
    /// <param name="query">Linq Query.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Number of elements in the query.</returns>
    Task<int> QueryCountAsync(IQueryable<TE> query, CancellationToken ct = default);

    /// <summary>
    /// Creates a List from a query.
    /// </summary>
    /// <param name="query">Linq Query.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of elements in the query.</returns>
    Task<List<TE>> QueryToListAsync(IQueryable<TE> query, CancellationToken ct = default);
}
