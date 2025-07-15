using System.Linq;

using Ardalis.Specification.EntityFrameworkCore;

using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.interfaces;

using Microsoft.EntityFrameworkCore;

namespace IAVH.BioTablero.CM.Persistence.Repositories;

public class Repository<T>(GeneralContext dbContext) : RepositoryBase<T>(dbContext), IRepository<T>
    where T : class, IAggregateRoot
{
    public IQueryable<T> GetQueryable() =>
        dbContext.Set<T>().AsNoTracking();
}
