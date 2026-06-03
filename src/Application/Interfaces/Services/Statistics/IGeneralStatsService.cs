namespace IAVH.BioTablero.CM.Application.Interfaces.Services.Statistics;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Domain;

/// <summary>
/// General statistics service interface.
/// </summary>
public interface IGeneralStatsService
{
    /// <summary>
    /// Get general statistics for community monitoring.
    /// </summary>
    /// <param name="departmentId">Department identifier (optional).</param>
    /// <param name="initiativeId">Initiative identifier (optional).</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>General statistics data.</returns>
    Task<CustomWebResponse> GetStatsAsync(int? departmentId = null, int? initiativeId = null, CancellationToken ct = default);
}
