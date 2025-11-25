namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.TerritoryStories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;
using IAVH.BioTablero.CM.Core.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.TerritoryStories;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Storage;

using Microsoft.EntityFrameworkCore;

using Serilog;

using InitiativeUserLevelEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.InitiativeUserLevel;

/// <summary>
/// Territory Story Image repository.
/// </summary>
public class TerritoryStoryImageRepository : Repository<TerritoryStoryImage, int>, ITerritoryStoryImageRepository
{
    private const string StoragePrefix = "territory-stories";
    private readonly ILogger logger;
    private readonly IStorageService storageService;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    /// <param name="logger">Logger.</param>
    /// <param name="storageService">Storage service.</param>
    public TerritoryStoryImageRepository(
        GeneralContext dbContext,
        ILogger logger,
        IStorageService storageService)
        : base(dbContext)
    {
        this.logger = logger;
        this.storageService = storageService;
    }

    /// <inheritdoc/>
    public async Task<bool> AuthorizedEntityReadAsync(int id, string userName, CancellationToken ct = default)
    {
        var initiative = await dbContext.Initiatives
            .Include(e => e.InitiativeUsers)
            .Include(e => e.TerritoryStories)
                .ThenInclude(e => e.Images)
            .Where(e => e.TerritoryStories.Any(e => e.Images.Any(e => e.Id == id)))
            .FirstOrDefaultAsync(ct);

        var userBelongsToInitiative = await dbContext.InitiativeUsers
            .Where(e => e.UserName == userName && e.InitiativeId == initiative.Id)
            .AnyAsync(ct);

        if (userBelongsToInitiative)
        {
            return await dbContext.TerritoryStoryImages
                .Where(e => e.Id == id)
                .AnyAsync(ct);
        }

        return await dbContext.TerritoryStoryImages
            .Include(e => e.TerritoryStory)
            .Where(e => e.Id == id && !e.TerritoryStory.Restricted)
            .AnyAsync(ct);
    }

    /// <summary>
    /// Check authorized user action.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities by selected territory story.</returns>
    public async Task<bool> AuthorizedEntityModifyAsync(int id, string userName, CancellationToken ct = default)
    {
        var territoryStory = await dbContext.TerritoryStoryImages
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
        await dbContext.TerritoryStoryImages
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
        await dbContext.TerritoryStoryImages
            .Where(e => e.Id != id && e.FileUrl == fileUrl)
            .AnyAsync(ct);

    /// <summary>
    /// Mark as featured content.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Updated territory story data.</returns>
    public async Task<TerritoryStoryImage> MarkAsFeaturedContentAsync(int id, CancellationToken ct = default)
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

    /// <summary>
    /// Adds an entity in the database and upload the image in the storage service.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="formFile">Image data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<TerritoryStoryImage> AddAsync(TerritoryStoryImage entity, IInputFile formFile, CancellationToken ct = default)
    {
        using var transaction = await dbContext.Database.BeginTransactionAsync(ct);

        try
        {
            await dbContext.TerritoryStoryImages.AddAsync(entity, ct);
            await dbContext.SaveChangesAsync(ct);

            // Upload/Overwrite image
            var fileName = $"{StoragePrefix}/{entity.Id}";
            var fileUri = new Uri($"{storageService.BaseUrl}/{fileName}");
            var uploadSuccessful = await storageService.UploadFileAsync(fileName, formFile, ct);

            if (!uploadSuccessful)
            {
                throw new StorageException("Territory Story Image upload error");
            }

            entity.FileUrl = fileUri;

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

    /// <summary>
    /// Update an entity in the database and upload the image in the storage service.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="formFile">Image data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<TerritoryStoryImage> UpdateAsync(TerritoryStoryImage entity, IInputFile formFile, CancellationToken ct = default)
    {
        using var transaction = await dbContext.Database.BeginTransactionAsync(ct);

        try
        {
            // Upload/Overwrite image
            var fileName = $"{StoragePrefix}/{entity.Id}";
            var fileUri = new Uri($"{storageService.BaseUrl}/{fileName}");
            var uploadSuccessful = await storageService.UploadFileAsync(fileName, formFile, ct);

            if (!uploadSuccessful)
            {
                throw new StorageException("Territory Story Image upload error");
            }

            entity.FileUrl = fileUri;

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
