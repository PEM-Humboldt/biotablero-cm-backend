namespace IAVH.BioTablero.CM.Application.Interfaces.Services.Reports;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Domain;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using Microsoft.AspNetCore.OData.Query;

/// <summary>
/// Monitoring Events service interface.
/// </summary>
public interface IMonitoringEventsService : IRead<MonitoringEvents, int>, IAdd<MonitoringEventsDto>, IUpdate<MonitoringEventsDto, int>, IDelete<int>
{
    /// <summary>
    /// Get elements list (OData).
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="queryOptions">OData query options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> GetListAsync(int initiativeId, ODataQueryOptions<MonitoringEvents> queryOptions, CancellationToken ct = default);
}
