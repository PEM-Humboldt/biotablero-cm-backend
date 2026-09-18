namespace IAVH.BioTablero.CM.Application.Interfaces.Services.Indicators;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.DTOs.Indicators;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;
using IAVH.BioTablero.CM.Core.Interfaces.ExternalServices;

/// <summary>
/// Observation service interface.
/// </summary>
public interface IObservationService : IRead<Observation, int>, IUpdate<ObservationDto, int>
{
    /// <summary>
    /// Get entities by initiative.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> GetByInitiativeAsync(int initiativeId, CancellationToken ct = default);

    /// <summary>
    /// Import observations.
    /// </summary>
    /// <param name="userName">User name.</param>
    /// <param name="requestData">Request data.</param>
    /// <param name="formFile">File data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> ImportObservationsAsync(string? userName, ObservationsImportFileDto requestData, IInputFile formFile, CancellationToken ct = default);
}
