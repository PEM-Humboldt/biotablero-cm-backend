namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using System.Linq;

using IAVH.BioTablero.CM.Core.Domain.Entities.Logging;

/// <summary>
/// Custom Log repository interface.
/// </summary>
public interface ILogRepository : IRepository<LogEntity>
{
    /// <summary>
    /// Include OData custom filters.
    /// </summary>
    /// <param name="query">Linq Query.</param>
    /// <returns>Modified Linq query.</returns>
    public IQueryable<LogEntity> IncludeOdataFilters(IQueryable<LogEntity> query);
}
