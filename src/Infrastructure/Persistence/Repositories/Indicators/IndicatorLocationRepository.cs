namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Indicators;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;
using IAVH.BioTablero.CM.Core.Domain.Models.Spreadsheets;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Indicators;
using IAVH.BioTablero.CM.Infrastructure.Persistence;
using IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;

using Serilog;

/// <summary>
/// Location repository.
/// </summary>
/// <param name="dbContext">General Database Context.</param>
/// <param name="logger">System logger.</param>
public class IndicatorLocationRepository(GeneralContext dbContext, ILogger logger) : Repository<IndicatorLocation, int>(dbContext, logger), IIndicatorLocationRepository
{
    /// <inheritdoc/>
    public async Task<List<IndicatorLocation>> GetByNamesAsync(LocationDataHelper[] locations, CancellationToken ct) =>
        await dbContext.IndicatorLocations
            .Include(e => e.Location)
                .ThenInclude(e => e.Parent)
            .Where(e => locations.Any(i => i.Municipality == e.Location.Name && i.Department == e.Location.Parent.Name && i.Locality == e.Locality))
            .ToListAsync(ct);
}
