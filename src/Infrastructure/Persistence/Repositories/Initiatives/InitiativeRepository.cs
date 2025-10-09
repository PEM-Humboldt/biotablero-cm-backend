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
public class InitiativeRepository : Repository<Initiative>, IInitiativeRepository
{
    private readonly GeneralContext dbContext;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    public InitiativeRepository(GeneralContext dbContext)
        : base(dbContext)
    {
        this.dbContext = dbContext;
    }

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
    public async Task<Point> GetCentroidAsync(int[] locationIds, CancellationToken ct = default)
    {
        var geometries = await dbContext.LocationPolygons
            .Where(lp => locationIds.Contains(lp.Id))
            .Select(lp => lp.Geometry)
            .ToListAsync(ct);

        var union = geometries.Aggregate((g1, g2) => g1.Union(g2));

        return union.Centroid;
    }

    /// <summary>
    /// Get count of active initiatives.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Count of active initiatives.</returns>
    public async Task<int> GetActiveInitiativesCountAsync(CancellationToken ct = default) =>
        await dbContext.Initiatives
            .Where(i => i.Enabled)
            .CountAsync(ct);

    /// <summary>
    /// Get total area of active initiatives with area.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Total area in square kilometers.</returns>
    public async Task<double> GetTotalAreaOfActiveInitiativesAsync(CancellationToken ct = default) =>
        await dbContext.Initiatives
            .Where(i => i.Enabled && i.PolygonArea > 0)
            .SumAsync(i => i.PolygonArea, ct);

    /// <summary>
    /// Get count of people involved in active initiatives.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Count of people involved in active initiatives.</returns>
    public async Task<int> GetPeopleInvolvedInActiveInitiativesCountAsync(CancellationToken ct = default) =>
        await dbContext.InitiativeUsers
            .Where(iu => iu.Initiative.Enabled)
            .CountAsync(ct);
}
