namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Resources;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Resources;

using Microsoft.EntityFrameworkCore;

using Serilog;

/// <summary>
/// Resource Tag repository.
/// </summary>
public class ResourceTagRepository : Repository<ResourceTag, int>, IResourceTagRepository
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    /// <param name="logger">System logger.</param>
    public ResourceTagRepository(
        GeneralContext dbContext,
        ILogger logger)
        : base(dbContext, logger)
    {
    }

    /// <inheritdoc/>
    public override async Task<ResourceTag> GetByIdAsync(int id, CancellationToken ct = default) =>
        await dbContext.ResourceTags
                .Include(e => e.Tag)
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync(ct);

    /// <inheritdoc/>
    public async Task<bool> IsDuplicatedAsync(int resourceId, int tagId, CancellationToken ct = default) =>
        await dbContext.ResourceTags
            .Where(e => e.ResourceId == resourceId && e.TagId == tagId)
            .AnyAsync(ct);

    /// <inheritdoc/>
    public override async Task<ResourceTag> AddAsync(ResourceTag entity, CancellationToken ct = default) =>
        await ExecuteInTransactionAsync(
            async ct =>
            {
                await dbContext.ResourceTags.AddAsync(entity, ct);
                await dbContext.SaveChangesAsync(ct);

                // Update Resource publication date
                var resource = await dbContext.Resources
                    .Where(e => e.Id == entity.ResourceId)
                    .FirstOrDefaultAsync(ct);

                if (!resource.IsDraft)
                {
                    resource.PublicationDate = DateTimeOffset.UtcNow;
                    await dbContext.SaveChangesAsync(ct);
                }

                return await GetByIdAsync(entity.Id, ct);
            },
            "Resource tag add transaction error",
            ct);

    /// <inheritdoc/>
    public override async Task<int> DeleteAsync(ResourceTag entity, CancellationToken ct = default) =>
        await ExecuteInTransactionAsync(
            async ct =>
            {
                dbContext.ResourceTags.Remove(entity);
                var result = await dbContext.SaveChangesAsync(ct);

                // Update Resource publication date
                var resource = await dbContext.Resources
                    .Where(e => e.Id == entity.ResourceId)
                    .FirstOrDefaultAsync(ct);

                if (!resource.IsDraft)
                {
                    resource.PublicationDate = DateTimeOffset.UtcNow;
                    await dbContext.SaveChangesAsync(ct);
                }

                return result;
            },
            "Resource tag delete transaction error",
            ct);
}
