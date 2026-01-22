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
/// Resource repository.
/// </summary>
public class ResourceRepository : Repository<Resource, int>, IResourceRepository
{
    private readonly IEmailResourceService emailResourceService;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="emailResourceService">Email resource service.</param>
    public ResourceRepository(
        GeneralContext dbContext,
        ILogger logger,
        IEmailResourceService emailResourceService)
        : base(dbContext, logger)
    {
        this.emailResourceService = emailResourceService;
    }

    /// <inheritdoc/>
    public async Task<IQueryable<Resource>> GetQueryWithInitiativeAndUserNameAsync(int initiativeId, string userName, IQueryable<Resource> query, CancellationToken ct = default)
    {
        var userBelongsToInitiative = await dbContext.InitiativeUsers
            .Where(e => e.UserName == userName && e.InitiativeId == initiativeId)
            .AnyAsync(ct);

        if (userBelongsToInitiative)
        {
            return IncludeCustomEntities(query)
                .Where(e => e.InitiativeId == initiativeId);
        }

        return IncludeCustomEntities(query)
            .Where(e => e.InitiativeId == initiativeId && !e.IsDraft);
    }

    /// <inheritdoc/>
    public async Task<bool> UserRelationshipExistsAsync(int id, string userName, CancellationToken ct = default) =>
        await dbContext.Resources
            .Include(e => e.Initiative)
                .ThenInclude(e => e.InitiativeUsers)
            .Where(e => e.Id == id && e.Initiative.InitiativeUsers.Any(e => e.UserName == userName))
            .AnyAsync(ct);

    /// <inheritdoc/>
    public async Task<bool> IsDuplicatedAsync(string name, CancellationToken ct = default) =>
        await dbContext.Resources
            .Where(e => e.Name == name)
            .AnyAsync(ct);

    /// <inheritdoc/>
    public async Task<bool> IsDuplicatedAsync(int id, string name, CancellationToken ct = default) =>
        await dbContext.Resources
            .Where(e => e.Id != id && e.Name == name)
            .AnyAsync(ct);

    /// <inheritdoc/>
    public async Task<Resource> UpdateAsync(Resource entity, string userName, CancellationToken ct = default) =>
        await ExecuteInTransactionAsync(
            async ct =>
            {
                entity.PublicationDate = DateTime.Now;
                await dbContext.SaveChangesAsync(ct);

                // Send notification
                var initiativeUsers = await dbContext.InitiativeUsers
                    .Where(e => e.InitiativeId == entity.InitiativeId)
                    .Select(e => e.UserName)
                    .ToArrayAsync(ct);

                var notificationSuccessfulProcess = await emailResourceService.SendNotificationUpdateResource(entity, userName, initiativeUsers, ct);

                if (!notificationSuccessfulProcess)
                {
                    logger.Error("Send resource update notification error");
                    throw new EmailException("Send resource update notification error");
                }

                return entity;
            },
            "Resource update transaction error",
            ct);

    /// <summary>
    /// Include custom entities.
    /// </summary>
    /// <returns>Modified Linq query.</returns>
    private IQueryable<Resource> IncludeCustomEntities(IQueryable<Resource> query = null)
    {
        query ??= dbContext.Resources;

        return query
            .Include(e => e.Likes)
            .Include(e => e.Files)
            .Include(e => e.Links)
            .Include(e => e.ResourceType)
            .Include(e => e.ResourceTags)
                .ThenInclude(e => e.Tag);
    }
}
