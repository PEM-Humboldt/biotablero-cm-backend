namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Resources;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Resources;

using Microsoft.EntityFrameworkCore;

using Serilog;

/// <summary>
/// Resource repository.
/// </summary>
public class ResourceRepository : Repository<Resource, int>, IResourceRepository
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    /// <param name="logger">System logger.</param>
    public ResourceRepository(
        GeneralContext dbContext,
        ILogger logger)
        : base(dbContext, logger)
    {
    }

    /// <inheritdoc/>
    public override async Task<Resource> GetByIdAsync(int id, CancellationToken ct = default) =>
        await IncludeCustomEntities()
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync(ct);

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
    public async Task<bool> AnyByTagAsync(int tagId, CancellationToken ct = default) =>
        await dbContext.Resources
            .Where(e => e.ResourceTags.Any(e => e.TagId == tagId))
            .AnyAsync(ct);

    /// <inheritdoc/>
    public override async Task<Resource> AddAsync(Resource entity, CancellationToken ct = default)
    {
        await base.AddAsync(entity, ct);
        return await GetByIdAsync(entity.Id, ct);
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
