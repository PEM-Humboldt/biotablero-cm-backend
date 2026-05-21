namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Initiatives;

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Models.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;

using Microsoft.EntityFrameworkCore;

using Serilog;

/// <summary>
/// Monitoring Events repository.
/// </summary>
public class MonitoringEventsRepository : Repository<MonitoringEvents, int>, IMonitoringEventsRepository
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    /// <param name="logger">System logger.</param>
    public MonitoringEventsRepository(
        GeneralContext dbContext,
        ILogger logger)
        : base(dbContext, logger)
    {
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<MonitoringEventsData>> GetMonitoringEventsData(int initiativeId, int? year = null, CancellationToken ct = default)
    {
        var query = dbContext.MonitoringEvents
            .Where(e =>
                e.InitiativeId == initiativeId &&
                (year == null || e.Date.Year == year));

        if (year.HasValue)
        {
            return await query
                .GroupBy(e => e.Date.Month)
                .Select(g => new MonitoringEventsData
                {
                    GroupNumber = g.Key,
                    GroupName = CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(g.Key),
                    Value = g.Sum(e => e.Value),
                })
                .OrderBy(e => e.GroupNumber)
                .ToListAsync(ct);
        }

        return await query
            .GroupBy(e => e.Date.Year)
            .Select(g => new MonitoringEventsData
            {
                GroupNumber = g.Key,
                GroupName = $"{g.Key}",
                Value = g.Sum(e => e.Value),
            })
            .OrderBy(e => e.GroupNumber)
            .ToListAsync(ct);
    }
}
