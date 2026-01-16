namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Resources;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;
using IAVH.BioTablero.CM.Core.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Resources;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Storage;

using Microsoft.EntityFrameworkCore;

using Serilog;

/// <summary>
/// Resource File repository.
/// </summary>
public class ResourceFileRepository : Repository<ResourceFile, int>, IResourceFileRepository
{
    private const string StoragePrefix = "resources";
    private readonly ILogger logger;
    private readonly IStorageService storageService;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    public ResourceFileRepository(GeneralContext dbContext)
        : base(dbContext)
    {
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ResourceFile>> GetByResourceAsync(int resourceId, CancellationToken ct = default) =>
        await dbContext.ResourceFiles
            .Where(e => e.ResourceId == resourceId)
            .ToListAsync(ct);

    /// <inheritdoc/>
    public async Task<ResourceFile> AddAsync(ResourceFile entity, IInputFile inputFile, CancellationToken ct = default)
    {
        using var transaction = await dbContext.Database.BeginTransactionAsync(ct);

        try
        {
            entity.Url = new Uri($"/temp-uri/{DateTime.Now.ToFileTime()}");
            await dbContext.ResourceFiles.AddAsync(entity, ct);
            await dbContext.SaveChangesAsync(ct);

            // Upload/Overwrite image
            var fileName = $"{StoragePrefix}/{entity.Id}.{inputFile.Extension}";
            var fileUri = new Uri($"{storageService.BaseUrl}/{fileName}");
            var uploadSuccessful = await storageService.UploadFileAsync(fileName, inputFile.OpenStream(), inputFile.ContentType, ct);

            if (!uploadSuccessful)
            {
                throw new StorageException("Resource file upload error");
            }

            entity.Url = fileUri;

            await dbContext.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            return entity;
        }
        catch (DbUpdateException ex)
        {
            await transaction.RollbackAsync(ct);
            logger.Error(ex, "Resource file transaction error");
            return null;
        }
        catch (StorageException ex)
        {
            await transaction.RollbackAsync(ct);
            logger.Error(ex, "Resource file transaction error");
            return null;
        }
    }

    /// <inheritdoc/>
    public async Task<ResourceFile> UpdateAsync(ResourceFile entity, IInputFile inputFile, CancellationToken ct = default)
    {
        using var transaction = await dbContext.Database.BeginTransactionAsync(ct);

        try
        {
            var oldFileUri = entity.Url;

            // Upload/Overwrite image
            var fileName = $"{StoragePrefix}/{entity.Id}.{inputFile.Extension}";
            var fileUri = new Uri($"{storageService.BaseUrl}/{fileName}");
            var uploadSuccessful = await storageService.UploadFileAsync(fileName, inputFile.OpenStream(), inputFile.ContentType, ct);

            if (!uploadSuccessful)
            {
                throw new StorageException("Resource file upload error");
            }

            entity.Url = fileUri;

            await dbContext.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            if (oldFileUri != entity.Url)
            {
                await storageService.DeleteFileAsync(oldFileUri.ToString(), ct);
            }

            return entity;
        }
        catch (DbUpdateException ex)
        {
            await transaction.RollbackAsync(ct);
            logger.Error(ex, "Resource file transaction error");
            return null;
        }
        catch (StorageException ex)
        {
            await transaction.RollbackAsync(ct);
            logger.Error(ex, "Resource file transaction error");
            return null;
        }
    }
}
