namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Locations;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Locations;

using Microsoft.EntityFrameworkCore;

using NetTopologySuite.Geometries;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.GeoEnums;

using LocationCustom = IAVH.BioTablero.CM.Core.Domain.Entities.Geo.Location;

/// <summary>
/// Location repository.
/// </summary>
/// <param name="dbContext">General Database Context.</param>
/// <param name="logger">System logger.</param>
public class LocationRepository(GeneralContext dbContext, ILogger logger) : Repository<LocationCustom, int>(dbContext, logger), ILocationRepository
{
    /// <inheritdoc/>
    public override async Task<LocationCustom> GetByIdAsync(int id, CancellationToken ct = default) =>
        await dbContext.Locations
            .Include(e => e.Parent)
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync(ct);

    /// <inheritdoc/>
    public async Task<IEnumerable<LocationCustom>> GetByParentIdAsync(int parentId, CancellationToken ct = default) =>
        await dbContext.Locations
            .Where(e => e.ParentId == parentId)
            .Include(e => e.Parent)
            .ToListAsync(ct);

    /// <inheritdoc/>
    public async Task<int> GetDepartmentIdByCoordinateAsync(Point coordinate, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(coordinate);

        var departmentId = await dbContext.Locations
            .Include(e => e.LocationPolygon)
            .Where(e =>
                e.Level == (byte)LocationLevel.Department &&
                e.LocationPolygon != null &&
                e.LocationPolygon.Geometry != null &&
                e.LocationPolygon.Geometry.Intersects(coordinate))
            .Select(e => e.Id)
            .FirstOrDefaultAsync(ct);

        return departmentId;
    }

    /// <inheritdoc/>
    public async Task<int> GetMunicipalitiesCountAsync(int initiativeId, CancellationToken ct = default) =>
        await dbContext.Locations
            .Include(e => e.InitiativeLocations)
            .Where(e => e.InitiativeLocations.Any(e => e.InitiativeId == initiativeId) && e.Level == (byte)LocationLevel.Municipality)
            .Distinct()
            .CountAsync(ct);
}
