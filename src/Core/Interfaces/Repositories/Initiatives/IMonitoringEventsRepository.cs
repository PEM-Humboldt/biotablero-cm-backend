namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;

using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Include OData custom entities.
    /// </summary>
    /// <param name="query">Linq Query.</param>
    /// <returns>Modified Linq query.</returns>
    IQueryable<MonitoringEvents> IncludeOdataEntities(IQueryable<MonitoringEvents> query);

    /// <summary>
    /// Add initiative filter.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="query">Linq Query.</param>
    /// <returns>Modified Linq query.</returns>
    IQueryable<MonitoringEvents> AddInitiativeFilter(int initiativeId, IQueryable<MonitoringEvents> query);

    /// <summary>
    /// Get Monitoring Events data.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="year">Year filter (optional).</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Monitirng Events data list.</returns>
    Task<IEnumerable<MonitoringEventsData>> GetMonitoringEventsData(int initiativeId, int? year = null, CancellationToken ct = default);

    /// <summary>
    /// Check if element is duplicated.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="monitoringEventsDate">Monitoring events date.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    Task<bool> IsDuplicatedAsync(int initiativeId, DateTimeOffset monitoringEventsDate, CancellationToken ct = default);

    /// <summary>
    /// Check if element is duplicated.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="monitoringEventsDate">Monitoring events date.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    Task<bool> IsDuplicatedAsync(int id, int initiativeId, DateTimeOffset monitoringEventsDate, CancellationToken ct = default);
}
