namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Resources;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Resources;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Resource File repository.
/// </summary>
public class ResourceFileRepository : Repository<ResourceFile, int>, IResourceFileRepository
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    public ResourceFileRepository(GeneralContext dbContext)
        : base(dbContext)
    {
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ResourceFile>> GetByResourceAsync(int resourceId, CancellationToken ct = default) =>
        await dbContext.ResourceFiles
            .Where(e => e.ResourceId == resourceId)
            .ToListAsync(ct);
}
