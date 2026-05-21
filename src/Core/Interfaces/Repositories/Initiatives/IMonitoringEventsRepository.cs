namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Models.Initiatives;

/// <summary>
/// Monitoring Events repository interface.
/// </summary>
public interface IMonitoringEventsRepository : IRepository<MonitoringEvents, int>
{
    /// <summary>
    /// Get Monitoring Events data.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="year">Year filter (optional).</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Monitirng Events data list.</returns>
    Task<IEnumerable<MonitoringEventsData>> GetMonitoringEventsData(int initiativeId, int? year = null, CancellationToken ct = default);
}
