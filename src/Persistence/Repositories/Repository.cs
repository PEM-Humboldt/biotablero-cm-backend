namespace IAVH.BioTablero.CM.Persistence.Repositories;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Ardalis.Specification.EntityFrameworkCore;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Base repository interface
/// </summary>
/// <typeparam name="T">Entity class type</typeparam>
public class Repository<T>(GeneralContext dbContext) : RepositoryBase<T>(dbContext), IRepository<T>
    where T : class, IAggregateRoot
{
    /// <summary>
    /// Get a new query
    /// </summary>
    /// <returns>Linq SQL Query</returns>
    public IQueryable<T> GetQueryable() =>
        dbContext.Set<T>().AsNoTracking();

    /// <summary>
    /// Get the number of elements in a query
    /// </summary>
    /// <param name="query">Linq SQL query</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Number of elements in the query</returns>
    public async Task<int> QueryCountAsync(IQueryable<T> query, CancellationToken ct = default) => await query.CountAsync(ct);

    /// <summary>
    /// Creates a List from a query
    /// </summary>
    /// <param name="query">Linq SQL query</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of elements in the query</returns>
    public async Task<List<T>> QueryToListAsync(IQueryable<T> query, CancellationToken ct = default) => await query.ToListAsync(ct);
}
