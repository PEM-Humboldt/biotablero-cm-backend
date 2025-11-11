namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.TerritoryStories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.TerritoryStories;

using Microsoft.EntityFrameworkCore;

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
