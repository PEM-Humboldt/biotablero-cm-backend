namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Locations;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Location repository.
/// </summary>
/// <param name="dbContext">General Database Context.</param>
public class LocationRepository(GeneralContext dbContext) : Repository<Location, int>(dbContext), ILocationRepository
{
    public async Task<IEnumerable<Location>> GetByParentIdAsync(int? parentId, CancellationToken ct = default) =>
        await dbContext.Locations
            .Where(i => i.ParentId == parentId)
            .ToListAsync(ct);
}
