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

/// <summary>
/// Territory Story Image repository.
/// </summary>
public class TerritoryStoryImageRepository : Repository<TerritoryStoryImage, int>, ITerritoryStoryImageRepository
{
    private readonly ILogger logger;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    /// <param name="logger">Logger.</param>
    public TerritoryStoryImageRepository(
        GeneralContext dbContext,
        ILogger logger)
        : base(dbContext)
    {
        this.logger = logger;
    }

    /// <summary>
    /// Get elements by territory story.
    /// </summary>
    /// <param name="territoryStoryId">Territory Story identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities by selected territory story.</returns>
    public async Task<IEnumerable<TerritoryStoryImage>> GetByTerritoryStoryAsync(int territoryStoryId, CancellationToken ct = default) =>
        await dbContext.TerritoryStoryImages
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

    /// <summary>
    /// Mark as featured content.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Updated territory story data.</returns>
    public async Task<TerritoryStoryImage> MarkAsFeaturedContent(int id, CancellationToken ct = default)
    {
        using var transaction = await dbContext.Database.BeginTransactionAsync(ct);

        try
        {
            var entity = await dbContext.TerritoryStoryImages
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

            var imagesFromSameStory = await dbContext.TerritoryStoryImages
                .Where(e => e.Id != entity.Id && e.TerritoryStoryId == entity.TerritoryStoryId)
                .ToListAsync(ct);

            foreach (var image in imagesFromSameStory)
            {
                image.FeaturedContent = false;
            }

            await dbContext.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            return entity;
        }
        catch (DbUpdateException ex)
        {
            await transaction.RollbackAsync(ct);
            logger.Error(ex, "Territory Story Image transaction error");
            return null;
        }
    }
}
