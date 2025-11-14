namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.TerritoryStories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.TerritoryStories;

using Microsoft.EntityFrameworkCore;

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
    public TerritoryStoryVideoRepository(GeneralContext dbContext)
        : base(dbContext)
    {
    }

    /// <summary>
    /// Check authorized user action.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities by selected territory story.</returns>
    public async Task<bool> AuthorizedUserAction(int id, string userName, CancellationToken ct = default)
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

    /// <summary>
    /// Get elements by territory story.
    /// </summary>
    /// <param name="territoryStoryId">Territory Story identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities by selected territory story.</returns>
    public async Task<IEnumerable<TerritoryStoryVideo>> GetByTerritoryStoryAsync(int territoryStoryId, CancellationToken ct = default) =>
        await dbContext.TerritoryStoryVideos
            .Where(e => e.TerritoryStoryId == territoryStoryId)
            .ToListAsync(ct);

    /// <summary>
    /// Check if element is duplicated.
    /// </summary>
    /// <param name="fileUrl">File URL.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    public async Task<bool> IsDuplicatedAsync(Uri fileUrl, CancellationToken ct = default) =>
        await dbContext.TerritoryStoryVideos
            .Where(e => e.FileUrl == fileUrl)
            .AnyAsync(ct);

    /// <summary>
    /// Check if element is duplicated.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="fileUrl">File URL.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    public async Task<bool> IsDuplicatedAsync(int id, Uri fileUrl, CancellationToken ct = default) =>
        await dbContext.TerritoryStoryVideos
            .Where(e => e.Id != id && e.FileUrl == fileUrl)
            .AnyAsync(ct);
}
