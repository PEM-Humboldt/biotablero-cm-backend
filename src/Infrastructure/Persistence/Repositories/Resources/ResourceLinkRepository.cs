namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Resources;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Resources;

using Microsoft.EntityFrameworkCore;

using Serilog;

/// <summary>
/// Resource Link repository.
/// </summary>
public class ResourceLinkRepository : Repository<ResourceLink, int>, IResourceLinkRepository
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    /// <param name="logger">System logger.</param>
    public ResourceLinkRepository(
        GeneralContext dbContext,
        ILogger logger)
        : base(dbContext, logger)
    {
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ResourceLink>> GetByResourceAsync(int resourceId, CancellationToken ct = default) =>
        await dbContext.ResourceLinks
            .Where(e => e.ResourceId == resourceId)
            .ToListAsync(ct);

    /// <inheritdoc/>
    public async Task<bool> IsDuplicatedAsync(int resourceId, Uri url, CancellationToken ct = default) =>
        await dbContext.ResourceLinks
            .Where(e => e.ResourceId == resourceId && e.Url == url)
            .AnyAsync(ct);

    /// <inheritdoc/>
    public async Task<bool> IsDuplicatedAsync(int id, int resourceId, Uri url, CancellationToken ct = default) =>
        await dbContext.ResourceLinks
            .Where(e => e.Id != id && e.ResourceId == resourceId && e.Url == url)
            .AnyAsync(ct);

    /// <inheritdoc/>
    public override async Task<ResourceLink> AddAsync(ResourceLink entity, CancellationToken ct = default) =>
        await ExecuteInTransactionAsync(
            async ct =>
            {
                await dbContext.ResourceLinks.AddAsync(entity, ct);
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

                return entity;
            },
            "Resource link add transaction error",
            ct);

    /// <inheritdoc/>
    public override async Task<int> UpdateAsync(ResourceLink entity, CancellationToken ct = default) =>
        await ExecuteInTransactionAsync(
            async ct =>
            {
                dbContext.Entry(entity).State = EntityState.Modified;
                var result = await dbContext.SaveChangesAsync(ct);

                // Update Resource publication date
                var resource = await dbContext.Resources
                    .Where(e => e.Id == entity.ResourceId)
                    .FirstOrDefaultAsync(ct);

                if (!resource.IsDraft)
                {
                    resource.PublicationDate = DateTimeOffset.UtcNow;
                    result = await dbContext.SaveChangesAsync(ct);
                }

                return result;
            },
            "Resource link update transaction error",
            ct);
}
