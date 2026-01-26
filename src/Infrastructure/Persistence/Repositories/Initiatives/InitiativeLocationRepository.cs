namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Initiatives;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;

using Microsoft.EntityFrameworkCore;

using Serilog;

/// <summary>
/// Initiative Location repository.
/// </summary>
public class InitiativeLocationRepository : Repository<InitiativeLocation, int>, IInitiativeLocationRepository
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    /// <param name="logger">System logger.</param>
    public InitiativeLocationRepository(
        GeneralContext dbContext,
        ILogger logger)
        : base(dbContext, logger)
    {
    }

    /// <inheritdoc/>
    public override async Task<InitiativeLocation> GetByIdAsync(int id, CancellationToken ct = default) =>
        await dbContext.InitiativeLocations
            .Include(e => e.Location)
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync(ct);

    /// <inheritdoc/>
    public async Task<IEnumerable<InitiativeLocation>> GetByInitiativeAsync(int initiativeId, CancellationToken ct = default) =>
        await dbContext.InitiativeLocations
            .Include(e => e.Location)
            .Where(e => e.InitiativeId == initiativeId)
            .ToListAsync(ct);

    /// <inheritdoc/>
    public async Task<int> CountByInitiativeAsync(int initiativeId, CancellationToken ct = default) =>
        await dbContext.InitiativeLocations
            .Where(e => e.InitiativeId == initiativeId)
            .CountAsync(ct);

    /// <inheritdoc/>
    public async Task<bool> IsDuplicatedAsync(int initiativeId, int locationId, string locality, CancellationToken ct = default) =>
        await dbContext.InitiativeLocations
            .Where(e => e.InitiativeId == initiativeId && e.LocationId == locationId && e.Locality == locality)
            .AnyAsync(ct);

    /// <inheritdoc/>
    public async Task<bool> IsDuplicatedAsync(int id, int initiativeId, int locationId, string locality, CancellationToken ct = default) =>
        await dbContext.InitiativeLocations
            .Where(e => e.Id != id && e.InitiativeId == initiativeId && e.LocationId == locationId && e.Locality == locality)
            .AnyAsync(ct);
}
