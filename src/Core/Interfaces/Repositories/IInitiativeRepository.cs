namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using System.Linq;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Custom Initiative repository interface.
/// </summary>
public interface IInitiativeRepository : IRepository<Initiative>
{
    /// <summary>
    /// Include OData custom entities.
    /// </summary>
    /// <param name="query">Linq Query.</param>
    /// <returns>Modified Linq query.</returns>
    public IQueryable<Initiative> IncludeOdataEntities(IQueryable<Initiative> query);
}
