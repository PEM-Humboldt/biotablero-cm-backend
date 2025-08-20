namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Ardalis.Specification.EntityFrameworkCore;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;
using IAVH.BioTablero.CM.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Base repository interface.
/// </summary>
/// <typeparam name="T">Entity class type.</typeparam>
public class Repository<T> : RepositoryBase<T>, IRepository<T>
    where T : class, IAggregateRoot
{
    private readonly GeneralContext dbContext;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    public Repository(GeneralContext dbContext)
        : base(dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <summary>
    /// Get a new query.
    /// </summary>
    /// <returns>Linq Query.</returns>
    public IQueryable<T> GetQueryable() =>
        dbContext.Set<T>().AsNoTracking();

    /// <summary>
    /// Get the number of elements in a query.
    /// </summary>
    /// <param name="query">Linq Query.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Number of elements in the query.</returns>
    public async Task<int> QueryCountAsync(IQueryable<T> query, CancellationToken ct = default) => await query.CountAsync(ct);

    /// <summary>
    /// Creates a List from a query.
    /// </summary>
    /// <param name="query">Linq Query.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of elements in the query.</returns>
    public async Task<List<T>> QueryToListAsync(IQueryable<T> query, CancellationToken ct = default) => await query.ToListAsync(ct);
}
