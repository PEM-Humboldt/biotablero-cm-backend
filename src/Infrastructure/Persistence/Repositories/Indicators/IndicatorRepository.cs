namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Indicators;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Indicators;

using Microsoft.EntityFrameworkCore;

using Serilog;

/// <summary>
/// Indicator repository.
/// </summary>
public class IndicatorRepository : Repository<Indicator, int>, IIndicatorRepository
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    /// <param name="logger">System logger.</param>
    public IndicatorRepository(
        GeneralContext dbContext,
        ILogger logger)
        : base(dbContext, logger)
    {
    }

    /// <inheritdoc/>
    public override async Task<Indicator> GetByIdAsync(int id, CancellationToken ct = default) =>
        await dbContext.Indicators
            .Include(e => e.Versions)
            .Include(e => e.Type)
            .Include(e => e.IndicatorTags)
                .ThenInclude(e => e.Tag)
            .Include(e => e.IndicatorLocations)
                .ThenInclude(e => e.Location)
                    .ThenInclude(e => e.Parent)
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync(ct);

    /// <inheritdoc/>
    public IQueryable<Indicator> IncludeOdataEntities(IQueryable<Indicator> query) =>
        query
            .Include(e => e.Type)
            .Include(e => e.IndicatorTags)
                .ThenInclude(e => e.Tag)
            .Include(e => e.Versions);

    /// <inheritdoc/>
    public async Task<int> CountAsync(int initiativeId, CancellationToken ct = default) =>
        await dbContext.Indicators
            .Include(e => e.Initiative)
            .Where(e => e.Initiative.Id == initiativeId)
            .CountAsync(ct);

    /// <inheritdoc/>
    public async Task<int[]> GetVersions(int id, CancellationToken ct = default) =>
        await dbContext.IndicatorVersions
            .Where(e => e.IndicatorId == id)
            .Select(e => e.Version)
            .ToArrayAsync(ct);
}
