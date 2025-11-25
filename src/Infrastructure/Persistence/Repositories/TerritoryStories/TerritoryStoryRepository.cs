namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.TerritoryStories;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.TerritoryStories;

using Microsoft.EntityFrameworkCore;

using Serilog;

using InitiativeUserLevelEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.InitiativeUserLevel;

/// <summary>
/// Territory Story repository.
/// </summary>
public class TerritoryStoryRepository : Repository<TerritoryStory, int>, ITerritoryStoryRepository
{
    private readonly ILogger logger;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    /// <param name="logger">Logger.</param>
    public TerritoryStoryRepository(
        GeneralContext dbContext,
        ILogger logger)
        : base(dbContext)
    {
        this.logger = logger;
    }

    /// <summary>
    /// Finds an entity with the given primary key value.
    /// </summary>
    /// <param name="id">The value of the primary key for the entity to be found.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public new async Task<TerritoryStory> GetByIdAsync(int id, CancellationToken ct = default) =>
        await IncludeCustomEntities()
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync(ct);

    /// <inheritdoc/>
    public async Task<bool> AuthorizedEntityReadAsync(int id, string userName, CancellationToken ct = default)
    {
        var userBelongsToInitiative = await dbContext.InitiativeUsers
            .Include(e => e.Initiative)
                .ThenInclude(e => e.TerritoryStories)
            .Where(e => e.UserName == userName && e.Initiative.TerritoryStories.Any(e => e.Id == id))
            .AnyAsync(ct);

        if (userBelongsToInitiative)
        {
            return await dbContext.TerritoryStories
                .Where(e => e.Id == id)
                .AnyAsync(ct);
        }

        return await dbContext.TerritoryStories
            .Where(e => e.Id == id && !e.Restricted)
            .AnyAsync(ct);
    }

    /// <inheritdoc/>
    public async Task<bool> AuthorizedEntityModifyAsync(int? id, string userName, CancellationToken ct = default)
    {
        if (!id.HasValue)
        {
            var initiativeUser = await dbContext.InitiativeUsers
                .Where(e => e.UserName == userName)
                .FirstOrDefaultAsync(ct);

            return initiativeUser?.LevelId is (int)InitiativeUserLevelEnum.Leader or (int)InitiativeUserLevelEnum.Member;
        }
        else
        {
            var territoryStory = await dbContext.TerritoryStories
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync(ct);

            var initiativeUser = await dbContext.InitiativeUsers
                .Where(e => e.InitiativeId == territoryStory.InitiativeId && e.UserName == userName)
                .FirstOrDefaultAsync(ct);

            return initiativeUser?.LevelId is (int)InitiativeUserLevelEnum.Leader || userName == territoryStory.AuthorUserName;
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TerritoryStory>> GetByInitiativeAndUserNameAsync(int initiativeId, string userName, CancellationToken ct = default)
    {
        var userBelongsToInitiative = await dbContext.InitiativeUsers
            .Where(e => e.UserName == userName && e.InitiativeId == initiativeId)
            .AnyAsync(ct);

        if (userBelongsToInitiative)
        {
            return await IncludeCustomEntities()
                .Where(e => e.InitiativeId == initiativeId)
                .ToListAsync(ct);
        }

        return await IncludeCustomEntities()
            .Where(e => e.InitiativeId == initiativeId && !e.Restricted)
            .ToListAsync(ct);
    }

    /// <summary>
    /// Check if element is duplicated.
    /// </summary>
    /// <param name="title">Entity title.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    public async Task<bool> IsDuplicatedAsync(string title, CancellationToken ct = default) =>
        await dbContext.TerritoryStories
            .Where(e => e.Title == title)
            .AnyAsync(ct);

    /// <summary>
    /// Check if element is duplicated.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="title">Entity title.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    public async Task<bool> IsDuplicatedAsync(int id, string title, CancellationToken ct = default) =>
        await dbContext.TerritoryStories
            .Where(e => e.Id != id && e.Title == title)
            .AnyAsync(ct);

    /// <summary>
    /// Mark as featured content.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Updated territory story data.</returns>
    public async Task<TerritoryStory> MarkAsFeaturedContentAsync(int id, CancellationToken ct = default)
    {
        using var transaction = await dbContext.Database.BeginTransactionAsync(ct);

        try
        {
            var entity = await dbContext.TerritoryStories
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync(ct);

            if (entity == null)
            {
                return entity;
            }

            if (!entity.FeaturedContent)
            {
                entity.FeaturedContent = true;
            }

            var storiesFromSameinitiative = await dbContext.TerritoryStories
                .Where(e => e.Id != entity.Id && e.InitiativeId == entity.InitiativeId)
                .ToListAsync(ct);

            foreach (var territoryStory in storiesFromSameinitiative)
            {
                territoryStory.FeaturedContent = false;
            }

            await dbContext.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            return entity;
        }
        catch (DbUpdateException ex)
        {
            await transaction.RollbackAsync(ct);
            logger.Error(ex, "Territory Story transaction error");
            return null;
        }
    }

    /// <summary>
    /// Include custom entities.
    /// </summary>
    /// <returns>Modified Linq query.</returns>
    private IQueryable<TerritoryStory> IncludeCustomEntities() =>
        dbContext.TerritoryStories
            .Include(e => e.Images)
            .Include(e => e.Videos)
            .Include(e => e.Likes);
}
