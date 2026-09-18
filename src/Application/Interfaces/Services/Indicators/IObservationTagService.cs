namespace IAVH.BioTablero.CM.Application.Interfaces.Services.Indicators;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Observation Tag service interface.
/// </summary>
public interface IObservationTagService : IDelete<int>
{
    /// <summary>
    /// Add element.
    /// </summary>
    /// <param name="observationId">Observation identifier.</param>
    /// <param name="tagId">Tag identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> AddAsync(int observationId, int tagId, CancellationToken ct = default);
}
