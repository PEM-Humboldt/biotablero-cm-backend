namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Resources;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Application.Interfaces.Services.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;
using IAVH.BioTablero.CM.Core.Domain.Models.Email;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Resources;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Email;

using Microsoft.EntityFrameworkCore;

using Serilog;

/// <summary>
/// Resource repository.
/// </summary>
public class ResourceRepository : Repository<Resource, int>, IResourceRepository
{
    private readonly ILogger logger;
    private readonly IWebViewTools webViewTools;
    private readonly IEmailService emailService;
    private readonly IIamService iamService;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="webViewTools">Web View Tools.</param>
    /// <param name="emailService">Email service.</param>
    /// <param name="iamService">IAM service.</param>
    public ResourceRepository(
        GeneralContext dbContext,
        ILogger logger,
        IWebViewTools webViewTools,
        IEmailService emailService,
        IIamService iamService)
        : base(dbContext)
    {
        this.logger = logger;
        this.webViewTools = webViewTools;
        this.emailService = emailService;
        this.iamService = iamService;
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
    public async Task<Resource> UpdateAsync(Resource entity, string userName, CancellationToken ct = default)
    {
        using var transaction = await dbContext.Database.BeginTransactionAsync(ct);

        try
        {
            // Update entity.
            entity.PublicationDate = DateTime.Now;
            await dbContext.SaveChangesAsync(ct);

            // Send notification
            var initiativeUsers = await dbContext.InitiativeUsers
                .Where(e => e.InitiativeId == entity.InitiativeId)
                .ToArrayAsync(ct);

            var notificationSuccessfulProcess = await SendNotificationUpdateResource(entity, userName, initiativeUsers, ct);

            if (!notificationSuccessfulProcess)
            {
                logger.Error("Send resource update notification error");
                throw new EmailException("Send resource update notification error");
            }

            await transaction.CommitAsync(ct);
            return entity;
        }
        catch (DbUpdateException ex)
        {
            await transaction.RollbackAsync(ct);
            logger.Error(ex, "Resource update transaction error");
            return null;
        }
        catch (EmailException ex)
        {
            await transaction.RollbackAsync(ct);
            logger.Error(ex, "Resource update transaction error");
            return null;
        }
    }

    /// <summary>
    /// Send notification for recourse update.
    /// </summary>
    /// <param name="resource">Resource data.</param>
    /// <param name="userName">Editor user name.</param>
    /// <param name="initiativeUsers">initiative users.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if the process is successful. False otherwise.</returns>
    private async Task<bool> SendNotificationUpdateResource(Resource resource, string userName, InitiativeUser[] initiativeUsers, CancellationToken ct = default)
    {
        var emailData = new UpdateResourceEmailData
        {
            ResourceName = resource.Name,
            EditorUserName = userName,
        };

        var initiativeUserNames = initiativeUsers
            .Select(e => e.UserName)
            .ToArray();

        var externalUsersData = await iamService.GetUsersDataAsync(initiativeUserNames, ct);

        var receivers = initiativeUsers
            .Select(e => new CustomEmailAddress(e.UserName))
            .ToArray();

        var htmlBody = await webViewTools.RenderViewToStringAsync("UpdateResource", emailData);

        var response = await emailService.SendEmailAsync(emailData.Subject, receivers, null, htmlBody, ct);

        return !string.IsNullOrEmpty(response);
    }

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
