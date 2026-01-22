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
    private readonly IStorageService storageService;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="storageService">Storage service.</param>
    public ResourceFileRepository(
        GeneralContext dbContext,
        ILogger logger,
        IStorageService storageService)
        : base(dbContext, logger)
    {
        this.storageService = storageService;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ResourceFile>> GetByResourceAsync(int resourceId, CancellationToken ct = default) =>
        await dbContext.ResourceFiles
            .Where(e => e.ResourceId == resourceId)
            .ToListAsync(ct);

    /// <inheritdoc/>
    public async Task<ResourceFile> AddAsync(ResourceFile entity, IInputFile inputFile, CancellationToken ct = default) =>
        await ExecuteInTransactionAsync(
            async ct =>
            {
                entity.Url = new Uri($"/temp-uri/{DateTime.Now.ToFileTime()}");
                await dbContext.ResourceFiles.AddAsync(entity, ct);
                await dbContext.SaveChangesAsync(ct);

                await UploadAndSetFileAsync(entity, inputFile, ct);
                await dbContext.SaveChangesAsync(ct);

                // Update Resource publication date
                var resource = await dbContext.Resources
                    .Where(e => e.Id == entity.ResourceId)
                    .FirstOrDefaultAsync(ct);

                if (!resource.IsDraft)
                {
                    resource.PublicationDate = DateTime.Now;
                    await dbContext.SaveChangesAsync(ct);
                }

                return entity;
            },
            "Resource file add transaction error",
            ct);

    /// <inheritdoc/>
    public new async Task<int> UpdateAsync(ResourceFile entity, CancellationToken ct = default) =>
        await ExecuteInTransactionAsync(
            async ct =>
            {
                dbContext.Entry(entity).State = EntityState.Modified;
                var result = await dbContext.SaveChangesAsync(ct);

                // Update Resource publication date
                var resource = await dbContext.Resources
                    .Where(e => e.Id == entity.ResourceId)
                    .FirstOrDefaultAsync(ct);

                if (!resource.IsDraft)
                {
                    resource.PublicationDate = DateTime.Now;
                    result = await dbContext.SaveChangesAsync(ct);
                }

                return result;
            },
            "Resource file update transaction error",
            ct);

    /// <inheritdoc/>
    public async Task<int> UpdateAsync(ResourceFile entity, IInputFile inputFile, CancellationToken ct = default) =>
        await ExecuteInTransactionAsync(
            async ct =>
            {
                var oldFileUri = entity.Url;

                await UploadAndSetFileAsync(entity, inputFile, ct);
                var result = await dbContext.SaveChangesAsync(ct);

                // Update Resource publication date
                var resource = await dbContext.Resources
                    .Where(e => e.Id == entity.ResourceId)
                    .FirstOrDefaultAsync(ct);

                if (!resource.IsDraft)
                {
                    resource.PublicationDate = DateTime.Now;
                    result = await dbContext.SaveChangesAsync(ct);
                }

                if (oldFileUri != entity.Url)
                {
                    await storageService.DeleteFileAsync(oldFileUri.ToString(), ct);
                }

                return result;
            },
            "Resource file update transaction error",
            ct);

    /// <summary>
    /// Upload and set Resource File.
    /// </summary>
    /// <param name="entity">Resource File entity.</param>
    /// <param name="inputFile">Input File data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    /// <exception cref="StorageException">External storage exception.</exception>
    private async Task UploadAndSetFileAsync(
        ResourceFile entity,
        IInputFile inputFile,
        CancellationToken ct)
    {
        var fileName = $"{StoragePrefix}/{entity.Id}.{inputFile.Extension}";
        var fileUri = new Uri($"{storageService.BaseUrl}/{fileName}");

        if (!await storageService.UploadFileAsync(
            fileName,
            inputFile.OpenStream(),
            inputFile.ContentType,
            ct))
        {
            throw new StorageException("Resource file upload error");
        }

        entity.Url = fileUri;
    }
}
