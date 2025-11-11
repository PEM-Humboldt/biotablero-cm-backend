namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Locations;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Locations;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Location Polygon repository.
/// </summary>
/// <param name="dbContext">General Database Context.</param>
public class LocationPolygonRepository(GeneralContext dbContext) : Repository<LocationPolygon, int>(dbContext), ILocationPolygonRepository
{
    /// <summary>
    /// Get simplified polygon by location identifier.
    /// </summary>
    /// <param name="locationId">Location identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Selected simplified polygon.</returns>
    public async Task<string> GetSimplifiedByLocationAsync(int locationId, CancellationToken ct = default) =>
        await dbContext.LocationPolygons
            .Where(i => i.LocationId == locationId)
            .Select(e => e.GeometrySimplified)
            .FirstOrDefaultAsync(ct);
}
