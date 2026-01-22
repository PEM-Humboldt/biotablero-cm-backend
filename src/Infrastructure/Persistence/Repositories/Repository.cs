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
using IAVH.BioTablero.CM.Infrastructure.Integrations.Email;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Storage;
using IAVH.BioTablero.CM.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

using Serilog;

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
    /// Serilog logger.
    /// </summary>
    private protected readonly ILogger logger;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    /// <param name="logger">System logger.</param>
    public Repository(
        GeneralContext dbContext,
        ILogger logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    /// <inheritdoc/>
    public async Task<TE> GetByIdAsync(TI id, CancellationToken ct = default) => await dbContext.Set<TE>().FindAsync([id], ct);

    /// <inheritdoc/>
    public async Task<List<TE>> ListAsync(CancellationToken ct = default) => await dbContext.Set<TE>().ToListAsync(ct);

    /// <inheritdoc/>
    public async Task<int> CountAsync(CancellationToken ct = default) => await dbContext.Set<TE>().CountAsync(ct);

    /// <inheritdoc/>
    public async Task<bool> AnyAsync(CancellationToken ct = default) => await dbContext.Set<TE>().AnyAsync(ct);

    /// <inheritdoc/>
    public async Task<bool> AnyAsync(TI id, CancellationToken ct = default)
    {
        var parameter = Expression.Parameter(typeof(TE), "e");
        var property = Expression.Property(parameter, "Id");
        var constant = Expression.Constant(id);
        var body = Expression.Equal(property, constant);

        var predicate = Expression.Lambda<Func<TE, bool>>(body, parameter);

        return await dbContext.Set<TE>().AnyAsync(predicate, ct);
    }

    /// <inheritdoc/>
    public async Task<TE> AddAsync(TE entity, CancellationToken ct = default)
    {
        dbContext.Set<TE>().Add(entity);
        await SaveChangesAsync(ct);
        return entity;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TE>> AddRangeAsync(IEnumerable<TE> entities, CancellationToken ct = default)
    {
        dbContext.Set<TE>().AddRange(entities);
        await SaveChangesAsync(ct);
        return entities;
    }

    /// <inheritdoc/>
    public async Task<int> UpdateAsync(TE entity, CancellationToken ct = default)
    {
        dbContext.Entry(entity).State = EntityState.Modified;
        var result = await SaveChangesAsync(ct);
        return result;
    }

    /// <inheritdoc/>
    public async Task<int> UpdateRangeAsync(IEnumerable<TE> entities, CancellationToken ct = default)
    {
        foreach (var entity in entities)
        {
            dbContext.Entry(entity).State = EntityState.Modified;
        }

        var result = await SaveChangesAsync(ct);
        return result;
    }

    /// <inheritdoc/>
    public async Task<int> DeleteAsync(TE entity, CancellationToken ct = default)
    {
        dbContext.Set<TE>().Remove(entity);
        var result = await SaveChangesAsync(ct);
        return result;
    }

    /// <inheritdoc/>
    public async Task<int> DeleteRangeAsync(IEnumerable<TE> entities, CancellationToken ct = default)
    {
        dbContext.Set<TE>().RemoveRange(entities);
        var result = await SaveChangesAsync(ct);
        return result;
    }

    /// <inheritdoc/>
    public async Task<int> SaveChangesAsync(CancellationToken ct = default) => await dbContext.SaveChangesAsync(ct);

    /// <inheritdoc/>
    public IQueryable<TE> GetQueryable() =>
        dbContext.Set<TE>().AsNoTracking();

    /// <inheritdoc/>
    public async Task<int> QueryCountAsync(IQueryable<TE> query, CancellationToken ct = default) => await query.CountAsync(ct);

    /// <inheritdoc/>
    public async Task<List<TE>> QueryToListAsync(IQueryable<TE> query, CancellationToken ct = default) => await query.ToListAsync(ct);

    /// <inheritdoc/>
    public async Task<TR> ExecuteInTransactionAsync<TR>(Func<CancellationToken, Task<TR>> action, string errorContext, CancellationToken ct = default)
    {
        using var transaction = await dbContext.Database.BeginTransactionAsync(ct);

        try
        {
            var result = await action(ct);
            await transaction.CommitAsync(ct);
            return result;
        }
        catch (Exception ex) when (
            ex is DbUpdateException or
            EmailException or
            StorageException)
        {
            await transaction.RollbackAsync(ct);
            logger.Error(ex, errorContext);
            return default;
        }
    }
}
