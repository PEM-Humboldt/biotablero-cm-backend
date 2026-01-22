namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.TerritoryStories;

using System;
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
/// Territory Story Video repository.
/// </summary>
public class TerritoryStoryVideoRepository : Repository<TerritoryStoryVideo, int>, ITerritoryStoryVideoRepository
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    /// <param name="logger">Logger.</param>
    public TerritoryStoryVideoRepository(
        GeneralContext dbContext,
        ILogger logger)
        : base(dbContext, logger)
    {
    }

    /// <inheritdoc/>
    public async Task<bool> AuthorizedEntityReadAsync(int id, string userName, CancellationToken ct = default)
    {
        var initiative = await dbContext.Initiatives
            .Include(e => e.InitiativeUsers)
            .Include(e => e.TerritoryStories)
                .ThenInclude(e => e.Videos)
            .Where(e => e.TerritoryStories.Any(e => e.Videos.Any(e => e.Id == id)))
            .FirstOrDefaultAsync(ct);

        var userBelongsToInitiative = await dbContext.InitiativeUsers
            .Where(e => e.UserName == userName && e.InitiativeId == initiative.Id)
            .AnyAsync(ct);

        if (userBelongsToInitiative)
        {
            return await dbContext.TerritoryStoryVideos
                .Where(e => e.Id == id)
                .AnyAsync(ct);
        }

        return await dbContext.TerritoryStoryVideos
            .Include(e => e.TerritoryStory)
            .Where(e => e.Id == id && !e.TerritoryStory.Restricted)
            .AnyAsync(ct);
    }

    /// <inheritdoc/>
    public async Task<bool> AuthorizedEntityModifyAsync(int id, string userName, CancellationToken ct = default)
    {
        var territoryStory = await dbContext.TerritoryStoryVideos
            .Include(e => e.TerritoryStory)
            .Where(e => e.Id == id)
            .Select(e => e.TerritoryStory)
            .FirstOrDefaultAsync(ct);

        var initiativeUser = await dbContext.InitiativeUsers
            .Where(e => e.InitiativeId == territoryStory.InitiativeId && e.UserName == userName)
            .FirstOrDefaultAsync(ct);

        return initiativeUser?.LevelId is (int)InitiativeUserLevelEnum.Leader || userName == territoryStory.AuthorUserName;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TerritoryStoryVideo>> GetByTerritoryStoryAsync(int territoryStoryId, CancellationToken ct = default) =>
        await dbContext.TerritoryStoryVideos
            .Where(e => e.TerritoryStoryId == territoryStoryId)
            .ToListAsync(ct);

    /// <inheritdoc/>
    public async Task<bool> IsDuplicatedAsync(Uri fileUrl, CancellationToken ct = default) =>
        await dbContext.TerritoryStoryVideos
            .Where(e => e.FileUrl == fileUrl)
            .AnyAsync(ct);

    /// <inheritdoc/>
    public async Task<bool> IsDuplicatedAsync(int id, Uri fileUrl, CancellationToken ct = default) =>
        await dbContext.TerritoryStoryVideos
            .Where(e => e.Id != id && e.FileUrl == fileUrl)
            .AnyAsync(ct);

    /// <inheritdoc/>
    public async Task<bool> AnyDuplicatedAsync(Uri[] urls, CancellationToken ct = default) =>
        await dbContext.TerritoryStoryVideos
            .Where(e => urls.Contains(e.FileUrl))
            .AnyAsync(ct);
}
