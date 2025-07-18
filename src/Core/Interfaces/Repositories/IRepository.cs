namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Ardalis.Specification;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// General repository interface
/// </summary>
/// <typeparam name="T">Entity class type</typeparam>
public interface IRepository<T> : IRepositoryBase<T>
    where T : class, IAggregateRoot
{
    IQueryable<T> GetQueryable();

    Task<int> QueryCountAsync(IQueryable<T> query, CancellationToken ct = default);

    Task<List<T>> QueryToListAsync(IQueryable<T> query, CancellationToken ct = default);
}
