namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Resources;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices.Email;
using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Resources;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Email;

using Microsoft.EntityFrameworkCore;

using Serilog;

/// <summary>
/// Resource Tag repository.
/// </summary>
public class ResourceTagRepository : Repository<ResourceTag, int>, IResourceTagRepository
{
    private readonly IEmailResourceService emailResourceService;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="emailResourceService">Email resource service.</param>
    public ResourceTagRepository(
        GeneralContext dbContext,
        ILogger logger,
        IEmailResourceService emailResourceService)
        : base(dbContext, logger)
    {
        this.emailResourceService = emailResourceService;
    }

    /// <inheritdoc/>
    public async Task<bool> IsDuplicatedAsync(int resourceId, int tagId, CancellationToken ct = default) =>
        await dbContext.ResourceTags
            .Where(e => e.ResourceId == resourceId && e.TagId == tagId)
            .AnyAsync(ct);

    /// <inheritdoc/>
    public async Task<ResourceTag> AddAsync(ResourceTag entity, string userName, CancellationToken ct = default) =>
        await ExecuteInTransactionAsync(
            async ct =>
            {
                await dbContext.ResourceTags.AddAsync(entity, ct);
                await dbContext.SaveChangesAsync(ct);

                // Update Resource publication date and send notification
                var resource = await dbContext.Resources
                    .Where(e => e.Id == entity.ResourceId)
                    .FirstOrDefaultAsync(ct);

                if (!resource.IsDraft)
                {
                    resource.PublicationDate = DateTime.Now;
                    await dbContext.SaveChangesAsync(ct);

                    var initiativeUsers = await dbContext.InitiativeUsers
                        .Where(e => e.InitiativeId == resource.Id)
                        .Select(e => e.UserName)
                        .ToArrayAsync(ct);

                    var notificationSuccessfulProcess = await emailResourceService.SendNotificationUpdateResource(resource, userName, initiativeUsers, ct);

                    if (!notificationSuccessfulProcess)
                    {
                        throw new EmailException("Send resource update notification error");
                    }
                }

                return entity;
            },
            "Resource tag add transaction error",
            ct);

    /// <inheritdoc/>
    public async Task<int> DeleteAsync(ResourceTag entity, string userName, CancellationToken ct = default) =>
        await ExecuteInTransactionAsync(
            async ct =>
            {
                dbContext.ResourceTags.Remove(entity);
                var result = await dbContext.SaveChangesAsync(ct);

                // Update Resource publication date and send notification
                var resource = await dbContext.Resources
                    .Where(e => e.Id == entity.ResourceId)
                    .FirstOrDefaultAsync(ct);

                if (!resource.IsDraft)
                {
                    resource.PublicationDate = DateTime.Now;
                    await dbContext.SaveChangesAsync(ct);

                    var initiativeUsers = await dbContext.InitiativeUsers
                        .Where(e => e.InitiativeId == resource.Id)
                        .Select(e => e.UserName)
                        .ToArrayAsync(ct);

                    var notificationSuccessfulProcess = await emailResourceService.SendNotificationUpdateResource(resource, userName, initiativeUsers, ct);

                    if (!notificationSuccessfulProcess)
                    {
                        throw new EmailException("Send resource update notification error");
                    }
                }

                return result;
            },
            "Resource tag delete transaction error",
            ct);
}
