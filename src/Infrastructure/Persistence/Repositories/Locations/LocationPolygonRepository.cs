namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Locations;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Locations;

using Microsoft.EntityFrameworkCore;

using NetTopologySuite.Geometries;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.GeoEnums;

using LocationCustom = IAVH.BioTablero.CM.Core.Domain.Entities.Geo.Location;

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

    /// <inheritdoc/>
    public async Task<LocationCustom> GetDepartmentByCoordinateAsync(Point coordinate, CancellationToken ct = default)
    {
        if (coordinate == null)
        {
            return null;
        }

        return await dbContext.Locations
            .Include(e => e.LocationPolygon)
            .Where(e =>
                e.Level == (byte)LocationLevel.Department &&
                e.LocationPolygon != null &&
                e.LocationPolygon.Geometry != null &&
                e.LocationPolygon.Geometry.Intersects(coordinate))
            .FirstOrDefaultAsync(ct);
    }
}
