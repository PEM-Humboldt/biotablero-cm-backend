namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Indicators;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Indicators;

using Microsoft.EntityFrameworkCore;

using Serilog;

/// <summary>
/// Indicator Version repository.
/// </summary>
public class IndicatorVersionRepository : Repository<IndicatorVersion, int>, IIndicatorVersionRepository
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    /// <param name="logger">System logger.</param>
    public IndicatorVersionRepository(
        GeneralContext dbContext,
        ILogger logger)
        : base(dbContext, logger)
    {
    }

    /// <inheritdoc/>
    public override async Task<IndicatorVersion> GetByIdAsync(int id, CancellationToken ct = default) =>
        await dbContext.IndicatorVersions
            .Include(e => e.Maps)
                .ThenInclude(e => e.Legends)
                    .ThenInclude(e => e.Items)
            .Include(e => e.Groups)
                .ThenInclude(e => e.Values)
                    .ThenInclude(e => e.MeasureUnit)
            .Include(e => e.Groups)
                .ThenInclude(e => e.Category)
                    .ThenInclude(e => e.Parent)
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync(ct);
}
