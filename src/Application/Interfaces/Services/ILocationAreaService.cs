namespace IAVH.BioTablero.CM.Application.Interfaces.Services;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Location area service interface.
/// </summary>
public interface ILocationAreaService
{
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
