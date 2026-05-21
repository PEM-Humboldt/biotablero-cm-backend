namespace IAVH.BioTablero.CM.Application.Services.Statistics;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Statistics;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;

/// <summary>
/// Initiative statistics service.
/// </summary>
/// <param name="entityRepository">Monitoring Events repository.</param>
public class InitiativeStatsService(IMonitoringEventsRepository entityRepository) : IInitiativeStatsService
{
    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetMonitoringEvents(int initiativeId, int? year, CancellationToken ct = default) =>
        new()
        {
            ResponseBody = await entityRepository.GetMonitoringEventsData(initiativeId, year, ct),
        };
}
