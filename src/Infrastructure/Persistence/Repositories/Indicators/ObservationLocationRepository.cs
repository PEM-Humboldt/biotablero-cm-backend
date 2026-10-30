namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Indicators;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Indicators;
using IAVH.BioTablero.CM.Infrastructure.Persistence;
using IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;

using Serilog;

/// <summary>
/// Observation Location repository.
/// </summary>
/// <param name="dbContext">General Database Context.</param>
/// <param name="logger">System logger.</param>
public class ObservationLocationRepository(GeneralContext dbContext, ILogger logger) : Repository<TopicLocation, int>(dbContext, logger), IObservationLocationRepository
{
    /// <inheritdoc/>
    public async Task<List<TopicLocation>> GetByObservationAsync(int observationId, CancellationToken ct = default) =>
        await dbContext.TopicLocations
            .Include(e => e.Location)
                .ThenInclude(e => e!.Parent)
            .Where(e => e.TopicId == observationId)
            .ToListAsync(ct);
}
