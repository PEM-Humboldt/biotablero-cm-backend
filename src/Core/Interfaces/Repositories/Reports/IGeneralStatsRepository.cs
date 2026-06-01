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
    /// Get the ecosystems involved in initiatives.
    /// </summary>
    /// <param name="departmentId">Department identifier (optional).</param>
    /// <param name="initiativeId">Initiative identifier (optional).</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Involved ecosystems.</returns>
    Task<List<Tag>> GetEcosystemsAsync(int? departmentId, int? initiativeId, CancellationToken ct = default);
}
