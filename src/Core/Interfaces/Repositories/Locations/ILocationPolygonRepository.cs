namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.Locations;

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
    Task<string> GetSimplifiedByLocationAsync(int locationId, CancellationToken ct = default);

    /// <summary>
    /// Calculate initiative area by associated municipalities.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Total area in square kilometers.</returns>
    Task<double> CalcInitiativeAreaByMunicipalitiesAsync(int initiativeId, CancellationToken ct = default);
}
