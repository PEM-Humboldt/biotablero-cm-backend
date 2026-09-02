namespace IAVH.BioTablero.CM.Infrastructure.Integrations.Storage;

using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices.Storage;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Models.Storage;

using Serilog;

/// <summary>
/// S3 storage Service.
/// </summary>
/// <param name="logger">Logger.</param>
/// <param name="client">AWS S3 client.</param>
public class StorageService(
    ILogger logger,
    IAmazonS3 client) : IStorageService
{
    private const string ProjectPreffix = "bt-cm";
    private readonly ILogger logger = logger;
    private readonly IAmazonS3 client = client;
    private readonly string endpointUrl = EnvUtils.GetRequiredEnv("S3_ENDPOINT_URL");
    private readonly string bucketName = EnvUtils.GetRequiredEnv("S3_BUCKET_NAME");

    /// <inheritdoc/>
    public Uri BaseUrl => new($"{endpointUrl}/{bucketName}/{ProjectPreffix}");

    /// <inheritdoc/>
    public async Task<bool> UploadFileAsync(string fileName, Stream fileStream, string contentType, CancellationToken ct = default)
    {
        try
        {
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = fileStream,
                Key = $"{ProjectPreffix}/{fileName}",
                BucketName = bucketName,
                ContentType = contentType,
                DisableDefaultChecksumValidation = true,
            };

            using var fileTransferUtility = new TransferUtility(client);
            await fileTransferUtility.UploadAsync(uploadRequest, ct);
            fileStream.Dispose();

            return true;
        }
        catch (AmazonS3Exception ex)
        {
            logger.Error(ex, "AWS S3 error while uploading file {FileName}", fileName);
        }
        catch (IOException ex)
        {
            logger.Error(ex, "I/O error while uploading file {FileName}", fileName);
        }
        catch (OperationCanceledException ex)
        {
            logger.Warning(ex, "Upload of file {FileName} was canceled", fileName);
        }

        return false;
    }

    /// <inheritdoc/>
    public async Task<FileData?> DownloadFileAsync(string fileName, CancellationToken ct = default)
    {
        MemoryStream? ms = null;
        var fileData = new FileData();

        try
        {
            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = $"{ProjectPreffix}/{fileName}",
            };

            using (var response = await client.GetObjectAsync(request, ct))
            {
                if (response.HttpStatusCode == HttpStatusCode.OK)
                {
                    fileData.MimeType = response.Headers.ContentType;
                    using (ms = new MemoryStream())
                    {
                        await response.ResponseStream.CopyToAsync(ms, ct);
                    }
                }
            }

            if (ms is null || ms.ToArray().Length < 1)
            {
                logger.Error("Error: Document {@DocName} not found", fileName);
                return null;
            }

            fileData.File = ms.ToArray();
            return fileData;
        }
        catch (AmazonS3Exception ex)
        {
            logger.Error(ex, "AWS S3 error while downloading file {FileName}", fileName);
        }
        catch (IOException ex)
        {
            logger.Error(ex, "I/O error while downloading file {FileName}", fileName);
        }
        catch (OperationCanceledException ex)
        {
            logger.Warning(ex, "Download of file {FileName} was canceled", fileName);
        }

        return null;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteFileAsync(string fileName, CancellationToken ct = default)
    {
        var deleteObjectRequest = new DeleteObjectRequest
        {
            BucketName = bucketName,
            Key = fileName.Replace($"{endpointUrl}/{bucketName}", string.Empty),
        };

        try
        {
            await client.DeleteObjectAsync(deleteObjectRequest, ct);
            return true;
        }
        catch (AmazonS3Exception ex)
        {
            logger.Error(ex, "AWS S3 error while deleting file {FileName}", fileName);
        }
        catch (IOException ex)
        {
            logger.Error(ex, "I/O error while deleting file {FileName}", fileName);
        }
        catch (OperationCanceledException ex)
        {
            logger.Warning(ex, "The deletion of file {FileName} was canceled", fileName);
        }

        return false;
    }
}
