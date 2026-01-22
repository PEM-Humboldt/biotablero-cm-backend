namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Locations;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Locations;

using Microsoft.EntityFrameworkCore;

using Serilog;

/// <summary>
/// Location Polygon repository.
/// </summary>
/// <param name="dbContext">General Database Context.</param>
/// <param name="logger">System logger.</param>
public class LocationPolygonRepository(GeneralContext dbContext, ILogger logger) : Repository<LocationPolygon, int>(dbContext, logger), ILocationPolygonRepository
{
    /// <inheritdoc/>
    public async Task<string> GetSimplifiedByLocationAsync(int locationId, CancellationToken ct = default) =>
        await dbContext.LocationPolygons
            .Where(i => i.LocationId == locationId)
            .Select(e => e.GeometrySimplified)
            .FirstOrDefaultAsync(ct);
}
