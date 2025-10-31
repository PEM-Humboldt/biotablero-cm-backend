namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Initiatives;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Initiative Location repository.
/// </summary>
public class InitiativeLocationRepository : Repository<InitiativeLocation, int>, IInitiativeLocationRepository
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    public InitiativeLocationRepository(GeneralContext dbContext)
        : base(dbContext)
    {
    }

    /// <summary>
    /// Get elements by initiative.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities by selected initiative.</returns>
    public async Task<IEnumerable<InitiativeLocation>> GetByInitiativeAsync(int initiativeId, CancellationToken ct = default) =>
        await dbContext.InitiativeLocations
            .Include(e => e.Location)
            .Where(e => e.InitiativeId == initiativeId)
            .ToListAsync(ct);

    /// <summary>
    /// Count elements by initiative.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<int> CountByInitiativeAsync(int initiativeId, CancellationToken ct = default) =>
        await dbContext.InitiativeLocations
            .Where(e => e.InitiativeId == initiativeId)
            .CountAsync(ct);

    /// <summary>
    /// Check if element is duplicated.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="locationId">Location identifier.</param>
    /// <param name="locality">Locality name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    public async Task<bool> IsDuplicatedAsync(int initiativeId, int locationId, string locality, CancellationToken ct = default) =>
        await dbContext.InitiativeLocations
            .Where(e => e.InitiativeId == initiativeId && e.LocationId == locationId && e.Locality == locality)
            .AnyAsync(ct);

    /// <summary>
    /// Check if element is duplicated.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="locationId">Location identifier.</param>
    /// <param name="locality">Locality name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    public async Task<bool> IsDuplicatedAsync(int id, int initiativeId, int locationId, string locality, CancellationToken ct = default) =>
        await dbContext.InitiativeLocations
            .Where(e => e.Id != id && e.InitiativeId == initiativeId && e.LocationId == locationId && e.Locality == locality)
            .AnyAsync(ct);
}
