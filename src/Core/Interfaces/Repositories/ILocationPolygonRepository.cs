namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;

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
    public Task<string> GetSimplifiedByLocationAsync(int locationId, CancellationToken ct = default);
}
