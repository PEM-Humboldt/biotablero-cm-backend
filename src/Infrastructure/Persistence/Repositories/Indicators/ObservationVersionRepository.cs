namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Indicators;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Indicators;

using Microsoft.EntityFrameworkCore;

using Serilog;

/// <summary>
/// Observation Version repository.
/// </summary>
/// <param name="dbContext">General Database Context.</param>
/// <param name="logger">System logger.</param>
public class ObservationVersionRepository(
    GeneralContext dbContext,
    ILogger logger) : Repository<TopicVersion, int>(dbContext, logger), IObservationVersionRepository
{
    /// <inheritdoc/>
    public override async Task<TopicVersion?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await dbContext.TopicVersions
            .Include(e => e.Maps!)
                .ThenInclude(e => e.Legends!)
                    .ThenInclude(e => e.Items)
            .Include(e => e.Groups!)
                .ThenInclude(e => e.Values!)
                    .ThenInclude(e => e!.IndicatorType)
            .Include(e => e.Groups!)
                .ThenInclude(e => e.Values!)
                    .ThenInclude(e => e!.CentralActor)
            .Include(e => e.Groups!)
                .ThenInclude(e => e.Category)
                    .ThenInclude(e => e!.Parent)
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync(ct);

    /// <inheritdoc/>
    public async Task<int> GetLastVersionAsync(int observationId, CancellationToken ct = default) =>
        await dbContext.TopicVersions
            .Where(e => e.TopicId == observationId)
            .OrderByDescending(e => e.Version)
            .Select(e => e.Version)
            .FirstOrDefaultAsync(ct);
}
