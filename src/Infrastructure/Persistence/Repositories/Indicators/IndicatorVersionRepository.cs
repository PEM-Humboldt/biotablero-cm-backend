namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Indicators;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Indicators;

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
}
