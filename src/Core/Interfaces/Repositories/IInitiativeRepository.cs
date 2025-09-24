namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using NetTopologySuite.Geometries;

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

    /// <summary>
    /// Get polygon centroid.
    /// </summary>
    /// <param name="locationIds">Location identifiers.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Polygon centroid.</returns>
    public Task<Point> GetCentroidAsync(int[] locationIds, CancellationToken ct = default);
}
