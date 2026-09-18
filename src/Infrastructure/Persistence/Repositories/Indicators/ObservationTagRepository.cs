namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Indicators;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Indicators;

using Microsoft.EntityFrameworkCore;

using Serilog;

/// <summary>
/// Observation Tag repository.
/// </summary>
/// <param name="dbContext">General Database Context.</param>
/// <param name="logger">System logger.</param>
public class ObservationTagRepository(
    GeneralContext dbContext,
    ILogger logger) : Repository<ObservationTag, int>(dbContext, logger), IObservationTagRepository
{
    /// <inheritdoc/>
    public override async Task<ObservationTag?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await dbContext.ObservationTags
            .Include(e => e.Tag)
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync(ct);

    /// <inheritdoc/>
    public override async Task<ObservationTag> AddAsync(ObservationTag entity, CancellationToken ct = default)
    {
        await base.AddAsync(entity, ct);
        return (await GetByIdAsync(entity.Id, ct))!;
    }

    /// <inheritdoc/>
    public async Task<bool> IsDuplicatedAsync(int observationId, int tagId, CancellationToken ct = default) =>
        await dbContext.ObservationTags
            .Where(e => e.ObservationId == observationId && e.TagId == tagId)
            .AnyAsync(ct);
}
