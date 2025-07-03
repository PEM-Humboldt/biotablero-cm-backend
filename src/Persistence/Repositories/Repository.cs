using Ardalis.Specification.EntityFrameworkCore;

using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.interfaces;

namespace IAVH.BioTablero.CM.Persistence.Repositories;

public class Repository<T>(GeneralContext dbContext) : RepositoryBase<T>(dbContext), IRepository<T>
    where T : class, IAggregateRoot
{ }
