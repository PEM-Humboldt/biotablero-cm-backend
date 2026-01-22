namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Resources;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices.Email;
using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;
using IAVH.BioTablero.CM.Core.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Resources;
using IAVH.BioTablero.CM.Infrastructure.Integrations.Email;
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
    private readonly IEmailResourceService emailResourceService;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    /// <param name="logger">System logger.</param>
    /// <param name="storageService">Storage service.</param>
    /// <param name="emailResourceService">Email resource service.</param>
    public ResourceFileRepository(
        GeneralContext dbContext,
        ILogger logger,
        IStorageService storageService,
        IEmailResourceService emailResourceService)
        : base(dbContext, logger)
    {
        this.storageService = storageService;
        this.emailResourceService = emailResourceService;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ResourceFile>> GetByResourceAsync(int resourceId, CancellationToken ct = default) =>
        await dbContext.ResourceFiles
            .Where(e => e.ResourceId == resourceId)
            .ToListAsync(ct);

    /// <inheritdoc/>
    public async Task<ResourceFile> AddAsync(ResourceFile entity, IInputFile inputFile, string userName, CancellationToken ct = default) =>
        await ExecuteInTransactionAsync(
            async ct =>
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

                // Update Resource publication date and send notification
                var resource = await dbContext.Resources
                    .Where(e => e.Id == entity.ResourceId)
                    .FirstOrDefaultAsync(ct);

                if (!resource.IsDraft)
                {
                    resource.PublicationDate = DateTime.Now;
                    await dbContext.SaveChangesAsync(ct);

                    var initiativeUsers = await dbContext.InitiativeUsers
                        .Where(e => e.InitiativeId == resource.Id)
                        .Select(e => e.UserName)
                        .ToArrayAsync(ct);

                    var notificationSuccessfulProcess = await emailResourceService.SendNotificationUpdateResource(resource, userName, initiativeUsers, ct);

                    if (!notificationSuccessfulProcess)
                    {
                        throw new EmailException("Send resource update notification error");
                    }
                }

                return entity;
            },
            "Resource file add transaction error",
            ct);

    /// <inheritdoc/>
    public async Task<ResourceFile> UpdateAsync(ResourceFile entity, IInputFile inputFile, string userName, CancellationToken ct = default) =>
        await ExecuteInTransactionAsync(
            async ct =>
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

                // Update Resource publication date and send notification
                var resource = await dbContext.Resources
                    .Where(e => e.Id == entity.ResourceId)
                    .FirstOrDefaultAsync(ct);

                if (!resource.IsDraft)
                {
                    resource.PublicationDate = DateTime.Now;
                    await dbContext.SaveChangesAsync(ct);

                    var initiativeUsers = await dbContext.InitiativeUsers
                        .Where(e => e.InitiativeId == resource.Id)
                        .Select(e => e.UserName)
                        .ToArrayAsync(ct);

                    var notificationSuccessfulProcess = await emailResourceService.SendNotificationUpdateResource(resource, userName, initiativeUsers, ct);

                    if (!notificationSuccessfulProcess)
                    {
                        throw new EmailException("Send resource update notification error");
                    }
                }

                if (oldFileUri != entity.Url)
                {
                    await storageService.DeleteFileAsync(oldFileUri.ToString(), ct);
                }

                return entity;
            },
            "Resource file update transaction error",
            ct);
}
