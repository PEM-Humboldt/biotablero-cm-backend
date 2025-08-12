namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Ardalis.Specification;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Base repository interface
/// </summary>
/// <typeparam name="T">Entity class type</typeparam>
public interface IRepository<T> : IRepositoryBase<T>
    where T : class, IAggregateRoot
{
    /// <summary>
    /// Get a new query
    /// </summary>
    /// <returns>Linq SQL Query</returns>
    IQueryable<T> GetQueryable();

    /// <summary>
    /// Get the number of elements in a query
    /// </summary>
    /// <param name="query">Linq SQL query</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Number of elements in the query</returns>
    Task<int> QueryCountAsync(IQueryable<T> query, CancellationToken ct = default);

    /// <summary>
    /// Creates a List from a query
    /// </summary>
    /// <param name="query">Linq SQL query</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of elements in the query</returns>
    Task<List<T>> QueryToListAsync(IQueryable<T> query, CancellationToken ct = default);
}
