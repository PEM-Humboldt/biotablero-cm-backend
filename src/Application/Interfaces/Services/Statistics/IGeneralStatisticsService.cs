namespace IAVH.BioTablero.CM.Application.Interfaces.Services.Statistics;

using System.Threading;
using System.Threading.Tasks;

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

    /// <summary>
    /// Get general statistics filtered by department.
    /// </summary>
    /// <param name="departmentId">Department identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>General statistics data for the department.</returns>
    Task<CustomWebResponse> GetGeneralStatisticsByDepartmentAsync(int departmentId, CancellationToken ct = default);

    /// <summary>
    /// Get general statistics filtered by initiative.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>General statistics data for the initiative.</returns>
    Task<CustomWebResponse> GetGeneralStatisticsByInitiativeAsync(int initiativeId, CancellationToken ct = default);
}
