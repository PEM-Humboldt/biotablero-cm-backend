namespace IAVH.BioTablero.CM.Application.Interfaces.Services;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Reports;
using IAVH.BioTablero.CM.Application.Utils;

/// <summary>
/// General statistics service interface.
/// </summary>
public interface IGeneralStatisticsService
{
    /// <summary>
    /// Get general statistics for community monitoring.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>General statistics data.</returns>
    Task<CustomWebResponse> GetGeneralStatisticsAsync(CancellationToken ct = default);
}
