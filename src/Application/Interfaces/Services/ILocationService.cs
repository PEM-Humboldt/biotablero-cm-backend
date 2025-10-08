namespace IAVH.BioTablero.CM.Application.Interfaces.Services;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;

/// <summary>
/// Location service interface.
/// </summary>
public interface ILocationService : IRead<Location, int>
{
    /// <summary>
    /// Get locations by parent.
    /// </summary>
    /// <param name="parentId">Parent identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> GetByParentAsync(int? parentId, CancellationToken ct = default);

    /// <summary>
    /// Get entity polygon (simplified).
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> GetPolygonAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Calculate total area for a collection of location IDs.
    /// </summary>
    /// <param name="locationIds">Collection of location IDs.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Total area in square kilometers.</returns>
    Task<double> CalculateTotalAreaForLocationsAsync(IEnumerable<int> locationIds, CancellationToken ct = default);

    /// <summary>
    /// Calculate area for a single location.
    /// </summary>
    /// <param name="locationId">Location ID.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Area in square kilometers.</returns>
    Task<double> CalculateAreaForLocationAsync(int locationId, CancellationToken ct = default);

    /// <summary>
    /// Calculate area for locations by their codes.
    /// </summary>
    /// <param name="locationCodes">Collection of location codes.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Total area in square kilometers.</returns>
    Task<double> CalculateTotalAreaForLocationCodesAsync(IEnumerable<string> locationCodes, CancellationToken ct = default);
}
