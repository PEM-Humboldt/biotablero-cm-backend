namespace IAVH.BioTablero.CM.Persistence.Repositories;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Ardalis.Specification.EntityFrameworkCore;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

public class Repository<T>(GeneralContext dbContext) : RepositoryBase<T>(dbContext), IRepository<T>
    where T : class, IAggregateRoot
{
    public IQueryable<T> GetQueryable() =>
        dbContext.Set<T>().AsNoTracking();

    public async Task<int> QueryCountAsync(IQueryable<T> query, CancellationToken ct = default) => await query.CountAsync(ct);

    public async Task<List<T>> QueryToListAsync(IQueryable<T> query, CancellationToken ct = default) => await query.ToListAsync(ct);
}
