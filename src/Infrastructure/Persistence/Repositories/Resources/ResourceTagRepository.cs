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
    private readonly ILogger logger;
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
        : base(dbContext)
    {
        this.logger = logger;
        this.emailResourceService = emailResourceService;
    }

    /// <inheritdoc/>
    public async Task<bool> IsDuplicatedAsync(int resourceId, int tagId, CancellationToken ct = default) =>
        await dbContext.ResourceTags
            .Where(e => e.ResourceId == resourceId && e.TagId == tagId)
            .AnyAsync(ct);

    /// <inheritdoc/>
    public async Task<ResourceTag> AddAsync(ResourceTag entity, string userName, CancellationToken ct = default)
    {
        using var transaction = await dbContext.Database.BeginTransactionAsync(ct);

        try
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

            await transaction.CommitAsync(ct);

            return entity;
        }
        catch (DbUpdateException ex)
        {
            await transaction.RollbackAsync(ct);
            logger.Error(ex, "Resource tag add transaction error (DbUpdateException)");
            return null;
        }
        catch (EmailException ex)
        {
            await transaction.RollbackAsync(ct);
            logger.Error(ex, "Resource tag add transaction error (EmailException)");
            return null;
        }
    }

    /// <inheritdoc/>
    public async Task<int> DeleteAsync(ResourceTag entity, string userName, CancellationToken ct = default)
    {
        using var transaction = await dbContext.Database.BeginTransactionAsync(ct);

        try
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

            await transaction.CommitAsync(ct);

            return result;
        }
        catch (DbUpdateException ex)
        {
            await transaction.RollbackAsync(ct);
            logger.Error(ex, "Resource tag delete transaction error (DbUpdateException)");
            return 0;
        }
        catch (EmailException ex)
        {
            await transaction.RollbackAsync(ct);
            logger.Error(ex, "Resource tag delete transaction error (EmailException)");
            return 0;
        }
    }
}
