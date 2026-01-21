namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Resources;

using System;
using System.Collections.Generic;
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
/// Resource Link repository.
/// </summary>
public class ResourceLinkRepository : Repository<ResourceLink, int>, IResourceLinkRepository
{
    private readonly ILogger logger;
    private readonly IEmailResourceService emailResourceService;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="emailResourceService">Email resource service.</param>
    public ResourceLinkRepository(
        GeneralContext dbContext,
        ILogger logger,
        IEmailResourceService emailResourceService)
        : base(dbContext)
    {
        this.logger = logger;
        this.emailResourceService = emailResourceService;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ResourceLink>> GetByResourceAsync(int resourceId, CancellationToken ct = default) =>
        await dbContext.ResourceLinks
            .Where(e => e.ResourceId == resourceId)
            .ToListAsync(ct);

    /// <inheritdoc/>
    public async Task<bool> IsDuplicatedAsync(Uri url, CancellationToken ct = default) =>
        await dbContext.ResourceLinks
            .Where(e => e.Url == url)
            .AnyAsync(ct);

    /// <inheritdoc/>
    public async Task<bool> IsDuplicatedAsync(int id, Uri url, CancellationToken ct = default) =>
        await dbContext.ResourceLinks
            .Where(e => e.Id != id && e.Url == url)
            .AnyAsync(ct);

    /// <inheritdoc/>
    public async Task<ResourceLink> AddAsync(ResourceLink entity, string userName, CancellationToken ct = default)
    {
        using var transaction = await dbContext.Database.BeginTransactionAsync(ct);

        try
        {
            await dbContext.ResourceLinks.AddAsync(entity, ct);
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

            await transaction.CommitAsync(ct);

            return entity;
        }
        catch (DbUpdateException ex)
        {
            await transaction.RollbackAsync(ct);
            logger.Error(ex, "Resource link add transaction error (DbUpdateException)");
            return null;
        }
        catch (EmailException ex)
        {
            await transaction.RollbackAsync(ct);
            logger.Error(ex, "Resource link update transaction error (EmailException)");
            return null;
        }
    }

    /// <inheritdoc/>
    public async Task<ResourceLink> UpdateAsync(ResourceLink entity, string userName, CancellationToken ct = default)
    {
        using var transaction = await dbContext.Database.BeginTransactionAsync(ct);

        try
        {
            dbContext.Entry(entity).State = EntityState.Modified;
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

            await transaction.CommitAsync(ct);

            return entity;
        }
        catch (DbUpdateException ex)
        {
            await transaction.RollbackAsync(ct);
            logger.Error(ex, "Resource link update transaction error (DbUpdateException)");
            return null;
        }
        catch (EmailException ex)
        {
            await transaction.RollbackAsync(ct);
            logger.Error(ex, "Resource link update transaction error (EmailException)");
            return null;
        }
    }
}
