namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Resources;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Resources;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Resource Link repository.
/// </summary>
public class ResourceLinkRepository : Repository<ResourceLink, int>, IResourceLinkRepository
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    public ResourceLinkRepository(GeneralContext dbContext)
        : base(dbContext)
    {
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ResourceLink>> GetByResourceAsync(int resourceId, CancellationToken ct = default) =>
        await dbContext.ResourceLinks
            .Where(e => e.ResourceId == resourceId)
            .ToListAsync(ct);
}
