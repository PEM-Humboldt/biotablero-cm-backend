namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Locations;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Locations;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Location repository.
/// </summary>
/// <param name="dbContext">General Database Context.</param>
public class LocationRepository(GeneralContext dbContext) : Repository<Location, int>(dbContext), ILocationRepository
{
    /// <summary>
    /// Finds an entity with the given primary key value.
    /// </summary>
    /// <param name="id">The value of the primary key for the entity to be found.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public new async Task<Location> GetByIdAsync(int id, CancellationToken ct = default) =>
        await dbContext.Locations
            .Include(e => e.Parent)
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync(ct);

    /// <summary>
    /// Get elements by parent identifier.
    /// </summary>
    /// <param name="parentId">Parent identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of elements by specified parent.</returns>
    public async Task<IEnumerable<Location>> GetByParentIdAsync(int? parentId, CancellationToken ct = default) =>
        await dbContext.Locations
            .Where(e => e.ParentId == parentId)
            .Include(e => e.Parent)
            .ToListAsync(ct);
}
