namespace IAVH.BioTablero.CM.Application.Interfaces.Services.Statistics;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Domain;

/// <summary>
/// Initiative statistics service interface.
/// </summary>
public interface IInitiativeStatsService
{
    /// <summary>
    /// Get monitoring events data.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier (optional).</param>
    /// <param name="year">Year filter (optional).</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Monitoring events data.</returns>
    Task<CustomWebResponse> GetMonitoringEvents(int initiativeId, int? year, CancellationToken ct = default);
}
