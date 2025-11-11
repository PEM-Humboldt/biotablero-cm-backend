namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;
using IAVH.BioTablero.CM.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Base repository interface.
/// </summary>
/// <typeparam name="TE">Entity class type.</typeparam>
/// <typeparam name="TI">Entity identifier type.</typeparam>
public class Repository<TE, TI> : IRepository<TE, TI>
    where TE : BaseEntity<TI>, IAggregateRoot
{
    /// <summary>
    /// General Database context.
    /// </summary>
    private protected readonly GeneralContext dbContext;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    public Repository(GeneralContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <summary>
    /// Finds an entity with the given primary key value.
    /// </summary>
    /// <param name="id">The value of the primary key for the entity to be found.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<TE> GetByIdAsync(TI id, CancellationToken ct = default) => await dbContext.Set<TE>().FindAsync([id], ct);

    /// <summary>
    /// Finds all entities of <typeparamref name="TE" /> from the database.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<List<TE>> ListAsync(CancellationToken ct = default) => await dbContext.Set<TE>().ToListAsync(ct);

    /// <summary>
    /// Returns the total number of records.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<int> CountAsync(CancellationToken ct = default) => await dbContext.Set<TE>().CountAsync(ct);

    /// <summary>
    /// Returns a boolean whether any entity exists or not.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<bool> AnyAsync(CancellationToken ct = default) => await dbContext.Set<TE>().AnyAsync(ct);

    /// <summary>
    /// Checks if entity with the given primary key value exists.
    /// </summary>
    /// <param name="id">The value of the primary key for the entity to be found.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<bool> AnyAsync(TI id, CancellationToken ct = default)
    {
        var parameter = Expression.Parameter(typeof(TE), "e");
        var property = Expression.Property(parameter, "Id");
        var constant = Expression.Constant(id);
        var body = Expression.Equal(property, constant);

        var predicate = Expression.Lambda<Func<TE, bool>>(body, parameter);

        return await dbContext.Set<TE>().AnyAsync(predicate, ct);
    }

    /// <summary>
    /// Adds an entity in the database.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<TE> AddAsync(TE entity, CancellationToken ct = default)
    {
        dbContext.Set<TE>().Add(entity);
        await SaveChangesAsync(ct);
        return entity;
    }

    /// <summary>
    /// Adds the given entities in the database.
    /// </summary>
    /// <param name="entities">The entities to add.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<IEnumerable<TE>> AddRangeAsync(IEnumerable<TE> entities, CancellationToken ct = default)
    {
        dbContext.Set<TE>().AddRange(entities);
        await SaveChangesAsync(ct);
        return entities;
    }

    /// <summary>
    /// Updates an entity in the database.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<int> UpdateAsync(TE entity, CancellationToken ct = default)
    {
        dbContext.Entry(entity).State = EntityState.Modified;
        var result = await SaveChangesAsync(ct);
        return result;
    }

    /// <summary>
    /// Updates the given entities in the database.
    /// </summary>
    /// <param name="entities">The entities to update.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<int> UpdateRangeAsync(IEnumerable<TE> entities, CancellationToken ct = default)
    {
        foreach (var entity in entities)
        {
            dbContext.Entry(entity).State = EntityState.Modified;
        }

        var result = await SaveChangesAsync(ct);
        return result;
    }

    /// <summary>
    /// Removes an entity in the database.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<int> DeleteAsync(TE entity, CancellationToken ct = default)
    {
        dbContext.Set<TE>().Remove(entity);
        var result = await SaveChangesAsync(ct);
        return result;
    }

    /// <summary>
    /// Removes the given entities in the database.
    /// </summary>
    /// <param name="entities">The entities to remove.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<int> DeleteRangeAsync(IEnumerable<TE> entities, CancellationToken ct = default)
    {
        dbContext.Set<TE>().RemoveRange(entities);
        var result = await SaveChangesAsync(ct);
        return result;
    }

    /// <summary>
    /// Persists changes to the database.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task<int> SaveChangesAsync(CancellationToken ct = default) => await dbContext.SaveChangesAsync(ct);

    /// <summary>
    /// Get a new query.
    /// </summary>
    /// <returns>Linq Query.</returns>
    public IQueryable<TE> GetQueryable() =>
        dbContext.Set<TE>().AsNoTracking();

    /// <summary>
    /// Get the number of elements in a query.
    /// </summary>
    /// <param name="query">Linq Query.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Number of elements in the query.</returns>
    public async Task<int> QueryCountAsync(IQueryable<TE> query, CancellationToken ct = default) => await query.CountAsync(ct);

    /// <summary>
    /// Creates a List from a query.
    /// </summary>
    /// <param name="query">Linq Query.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of elements in the query.</returns>
    public async Task<List<TE>> QueryToListAsync(IQueryable<TE> query, CancellationToken ct = default) => await query.ToListAsync(ct);
}
