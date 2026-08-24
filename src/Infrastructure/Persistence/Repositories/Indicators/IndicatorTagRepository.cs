namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Indicators;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Indicators;

using Microsoft.EntityFrameworkCore;

using Serilog;

/// <summary>
/// Initiative Tag repository.
/// </summary>
public class IndicatorTagRepository : Repository<IndicatorTag, int>, IIndicatorTagRepository
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    /// <param name="logger">System logger.</param>
    public IndicatorTagRepository(
        GeneralContext dbContext,
        ILogger logger)
        : base(dbContext, logger)
    {
    }

    /// <inheritdoc/>
    public override async Task<IndicatorTag?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await dbContext.IndicatorTags
            .Include(e => e.Tag)
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync(ct);

    /// <inheritdoc/>
    public override async Task<IndicatorTag> AddAsync(IndicatorTag entity, CancellationToken ct = default)
    {
        await base.AddAsync(entity, ct);
        return (await GetByIdAsync(entity.Id, ct))!;
    }

    /// <inheritdoc/>
    public async Task<bool> IsDuplicatedAsync(int indicatorId, int tagId, CancellationToken ct = default) =>
        await dbContext.IndicatorTags
            .Where(e => e.IndicatorId == indicatorId && e.TagId == tagId)
            .AnyAsync(ct);
}
