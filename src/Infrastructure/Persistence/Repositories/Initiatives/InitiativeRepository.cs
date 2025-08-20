namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Initiatives;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using Microsoft.EntityFrameworkCore;

using NetTopologySuite.Geometries;

/// <summary>
/// Custom Initiative repository.
/// </summary>
/// <param name="dbContext">General Database Context.</param>
public class InitiativeRepository(GeneralContext dbContext) : Repository<Initiative>(dbContext), IInitiativeRepository
{
    /// <summary>
    /// Include OData custom entities.
    /// </summary>
    /// <param name="query">Linq Query.</param>
    /// <returns>Modified Linq query.</returns>
    public IQueryable<Initiative> IncludeOdataEntities(IQueryable<Initiative> query) =>
        query
            .Include(e => e.InitiativeLocations)
                .ThenInclude(e => e.Location)
                    .ThenInclude(e => e.Parent);

    /// <summary>
    /// Get polygon centroid.
    /// </summary>
    /// <param name="locationIds">Location identifiers.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Polygon centroid.</returns>
    public async Task<Point> GetCentroid(int[] locationIds, CancellationToken ct = default)
    {
        var geometries = await dbContext.LocationPolygons
            .Where(lp => locationIds.Contains(lp.Id))
            .Select(lp => lp.Geometry)
            .ToListAsync(ct);

        var union = geometries.Aggregate((g1, g2) => g1.Union(g2));

        return union.Centroid;
    }
}
