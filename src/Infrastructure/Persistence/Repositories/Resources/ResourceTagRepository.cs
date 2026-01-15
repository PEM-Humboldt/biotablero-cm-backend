namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Resources;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Resources;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Resource Tag repository.
/// </summary>
public class ResourceTagRepository : Repository<ResourceTag, int>, IResourceTagRepository
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    public ResourceTagRepository(GeneralContext dbContext)
        : base(dbContext)
    {
    }

    /// <inheritdoc/>
    public async Task<bool> IsDuplicatedAsync(int resourceId, int tagId, CancellationToken ct = default) =>
        await dbContext.ResourceTags
            .Where(e => e.ResourceId == resourceId && e.TagId == tagId)
            .AnyAsync(ct);
}
