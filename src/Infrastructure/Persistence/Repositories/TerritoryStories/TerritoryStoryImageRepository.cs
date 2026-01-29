namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.TerritoryStories;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;
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
        : base(dbContext, logger)
    {
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

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public async Task<IEnumerable<TerritoryStoryImage>> GetByTerritoryStoryAsync(int territoryStoryId, CancellationToken ct = default) =>
        await dbContext.TerritoryStoryImages
            .Where(e => e.TerritoryStoryId == territoryStoryId)
            .ToListAsync(ct);

    /// <inheritdoc/>
    public async Task<TerritoryStoryImage> MarkAsFeaturedContentAsync(int id, CancellationToken ct = default) =>
        await ExecuteInTransactionAsync(
            async ct =>
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

                return entity;
            },
            "Territory Story Image transaction error",
            ct);

    /// <inheritdoc/>
    public async Task<TerritoryStoryImage> AddAsync(TerritoryStoryImage entity, Stream imageStream, string contentType, CancellationToken ct = default) =>
        await ExecuteInTransactionAsync(
            async ct =>
            {
                await UploadImage(entity, imageStream, contentType, ct);
                await dbContext.SaveChangesAsync(ct);

                return entity;
            },
            "Territory Story Image transaction error",
            ct);

    /// <inheritdoc/>
    public async Task<TerritoryStoryImage> UpdateAsync(TerritoryStoryImage entity, Stream imageStream, string contentType, CancellationToken ct = default) =>
        await ExecuteInTransactionAsync(
            async ct =>
            {
                await UploadImage(entity, imageStream, contentType, ct);
                return entity;
            },
            "Territory Story Image transaction error",
            ct);

    /// <summary>
    /// Upload Territory Story Image.
    /// </summary>
    /// <param name="entity">Territory Story Image entity.</param>
    /// <param name="imageStream">Image stream.</param>
    /// <param name="contentType">File content type.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    /// <exception cref="StorageException">Storage exception.</exception>
    private async Task UploadImage(TerritoryStoryImage entity, Stream imageStream, string contentType, CancellationToken ct = default)
    {
        var oldFileUri = entity.FileUrl;

        if (entity.FileUrl == null)
        {
            entity.FileUrl = new Uri($"/temp-uri/{DateTime.Now.ToFileTime()}");
            await dbContext.TerritoryStoryImages.AddAsync(entity, ct);
            await dbContext.SaveChangesAsync(ct);
        }

        // Upload/Overwrite image
        var fileName = $"{StoragePrefix}/{entity.Id}/{FileUtils.ComputeFileHash(imageStream)}.webp";
        var fileUri = new Uri($"{storageService.BaseUrl}/{fileName}");
        var uploadSuccessful = await storageService.UploadFileAsync(fileName, imageStream, contentType, ct);

        if (!uploadSuccessful)
        {
            throw new StorageException("Territory Story Image upload error");
        }

        entity.FileUrl = fileUri;

        await dbContext.SaveChangesAsync(ct);

        if (oldFileUri != null && oldFileUri != entity.FileUrl)
        {
            await storageService.DeleteFileAsync(oldFileUri.ToString(), ct);
        }
    }
}
