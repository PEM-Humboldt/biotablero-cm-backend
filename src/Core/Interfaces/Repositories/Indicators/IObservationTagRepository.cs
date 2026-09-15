namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.Indicators;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

/// <summary>
/// Observation Tag repository interface.
/// </summary>
public interface IObservationTagRepository : IRepository<ObservationTag, int>
{
    /// <summary>
    /// Check if element is duplicated.
    /// </summary>
    /// <param name="observationId">Observation identifier.</param>
    /// <param name="tagId">Tag identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    Task<bool> IsDuplicatedAsync(int observationId, int tagId, CancellationToken ct = default);
}
