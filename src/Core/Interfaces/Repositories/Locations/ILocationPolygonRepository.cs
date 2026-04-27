namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.Locations;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;

using NetTopologySuite.Geometries;

using LocationCustom = IAVH.BioTablero.CM.Core.Domain.Entities.Geo.Location;

/// <summary>
/// Location Polygon repository interface.
/// </summary>
public interface ILocationPolygonRepository : IRepository<LocationPolygon, int>
{
    /// <summary>
    /// Get simplified polygon by location identifier.
    /// </summary>
    /// <param name="locationId">Location identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Selected simplified polygon.</returns>
    Task<string> GetSimplifiedByLocationAsync(int locationId, CancellationToken ct = default);

    /// <summary>
    /// Get department by coordinate.
    /// </summary>
    /// <param name="coordinate">Geographic coordinate.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The department to wich the coordinate belongs. Null if doesn't belong to any department.</returns>
    Task<LocationCustom> GetDepartmentByCoordinateAsync(Point coordinate, CancellationToken ct = default);
}
