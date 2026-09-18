namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.Indicators;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

/// <summary>
/// Observation Location repository interface.
/// </summary>
public interface IObservationLocationRepository : IRepository<ObservationLocation, int>
{
    /// <summary>
    /// Get elements by observation identifier.
    /// </summary>
    /// <param name="observationId">Observation identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities by selected Observation.</returns>
    Task<List<ObservationLocation>> GetByObservationAsync(int observationId, CancellationToken ct = default);
}
