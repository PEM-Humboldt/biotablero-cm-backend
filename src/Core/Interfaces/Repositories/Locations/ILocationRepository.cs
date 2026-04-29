namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.Locations;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using NetTopologySuite.Geometries;

using LocationCustom = IAVH.BioTablero.CM.Core.Domain.Entities.Geo.Location;

/// <summary>
/// Location repository interface.
/// </summary>
public interface ILocationRepository : IRepository<LocationCustom, int>
{
    /// <summary>
    /// Get elements by parent identifier.
    /// </summary>
    /// <param name="parentId">Parent identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of elements by specified parent.</returns>
    Task<IEnumerable<LocationCustom>> GetByParentIdAsync(int parentId, CancellationToken ct = default);

    /// <summary>
    /// Get department identifier by coordinate.
    /// </summary>
    /// <param name="coordinate">Geographic coordinate.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The department to wich the coordinate belongs.</returns>
    Task<int> GetDepartmentIdByCoordinateAsync(Point coordinate, CancellationToken ct = default);
}
