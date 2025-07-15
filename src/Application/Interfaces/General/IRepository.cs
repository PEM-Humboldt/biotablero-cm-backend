using System.Linq;

using Ardalis.Specification;

using IAVH.BioTablero.CM.Core.interfaces;

namespace IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// General repository interface
/// </summary>
/// <typeparam name="T">Entity class type</typeparam>
public interface IRepository<T> : IRepositoryBase<T>
    where T : class, IAggregateRoot
{
    public IQueryable<T> GetQueryable();
}
