namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.Reports;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Tags;

/// <summary>
/// General Statistics repository interface.
/// </summary>
public interface IGeneralStatsRepository
{
    /// <summary>
    /// Get the number of enabled records.
    /// </summary>
    /// <param name="userName">User name.</param>
    /// <param name="departmentId">Department identifier (optional).</param>
    /// <param name="initiativeId">Initiative identifier (optional).</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Number of enabled records.</returns>
    Task<int> GetEnabledRecordsCountAsync(string userName = null, int? departmentId = null, int? initiativeId = null, CancellationToken ct = default);

    /// <summary>
    /// Get initiative polygon areas.
    /// </summary>
    /// <param name="departmentId">Department identifier (optional).</param>
    /// <param name="initiativeId">Initiative identifier (optional).</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Area in square kilometers.</returns>
    Task<double> GetAreaAsync(int? departmentId = null, int? initiativeId = null, CancellationToken ct = default);

    /// <summary>
    /// Get the number of people involved in initiatives.
    /// </summary>
    /// <param name="departmentId">Department identifier (optional).</param>
    /// <param name="initiativeId">Initiative identifier (optional).</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Number of involved people.</returns>
    Task<int> GetPeopleInvolvedCountAsync(int? departmentId = null, int? initiativeId = null, CancellationToken ct = default);

    /// <summary>
    /// Get the number of agreements involved in initiatives.
    /// </summary>
    /// <param name="departmentId">Department identifier (optional).</param>
    /// <param name="initiativeId">Initiative identifier (optional).</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Number of involved agreements.</returns>
    Task<int> GetAgreementsInvolvedCountAsync(int? departmentId, int? initiativeId, CancellationToken ct = default);

    /// <summary>
    /// Get the ecosystems involved in initiatives.
    /// </summary>
    /// <param name="departmentId">Department identifier (optional).</param>
    /// <param name="initiativeId">Initiative identifier (optional).</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Involved ecosystems.</returns>
    Task<List<Tag>> GetEcosystemsAsync(int? departmentId, int? initiativeId, CancellationToken ct = default);
}
