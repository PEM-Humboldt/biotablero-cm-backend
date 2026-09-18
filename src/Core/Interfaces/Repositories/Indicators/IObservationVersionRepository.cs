namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.Indicators;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

/// <summary>
/// Observation Version repository interface.
/// </summary>
public interface IObservationVersionRepository : IRepository<ObservationVersion, int>
{
    /// <summary>
    /// Get last version number.
    /// </summary>
    /// <param name="observationId">Observation identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The last version for the specified observation.</returns>
    Task<int> GetLastVersionAsync(int observationId, CancellationToken ct = default);
}
