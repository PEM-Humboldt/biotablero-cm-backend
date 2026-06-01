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
    /// Get general statistics.
    /// </summary>
    /// <param name="departmentId">Department identifier (optional).</param>
    /// <param name="initiativeId">Initiative identifier (optional).</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>General statistics data.</returns>
    Task<CustomWebResponse> GetGeneralStatsAsync(int? departmentId = null, int? initiativeId = null, CancellationToken ct = default);

    /// <summary>
    /// Get ecosystems statistics.
    /// </summary>
    /// <param name="departmentId">Department identifier (optional).</param>
    /// <param name="initiativeId">Initiative identifier (optional).</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>General statistics data.</returns>
    Task<CustomWebResponse> GetEcosystemsStatsAsync(int? departmentId = null, int? initiativeId = null, CancellationToken ct = default);

    /// <summary>
    /// Get demographic data from IAM system.
    /// </summary>
    /// <param name="departmentId">Department identifier (optional).</param>
    /// <param name="initiativeId">Initiative identifier (optional).</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Demographic data.</returns>
    Task<CustomWebResponse> GetDemographicData(int? departmentId = null, int? initiativeId = null, CancellationToken ct = default);
}
