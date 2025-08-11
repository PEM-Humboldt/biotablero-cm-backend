namespace IAVH.BioTablero.CM.Infrastructure.Integrations.Storage;

using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

using IAVH.BioTablero.CM.Core.Domain.Entities.Storage;
using IAVH.BioTablero.CM.Core.Interfaces.ExternalServices;

using Serilog;

/// <summary>
/// S3 storage Service.
/// </summary>
public class StorageService : IStorageService
{
    private readonly ILogger logger;
    private readonly AmazonS3Client client;
    private readonly string containerName;

    /// <summary>
    /// Service constructor.
    /// </summary>
    /// <param name="logger">Logger.</param>
    public StorageService(ILogger logger)
    {
        this.logger = logger;
        containerName = Environment.GetEnvironmentVariable("S3_BUCKET_NAME");
        client = InitClient();
    }

    /// <summary>
    /// Upload file.
    /// </summary>
    /// <param name="fileName">File name.</param>
    /// <param name="file">Upload file data.</param>
    /// <param name="ct">Cancellation token (optional).</param>
    /// <returns>True if the process is successful. False otherwise.</returns>
    public async Task<bool> UploadFile(string fileName, IInputFile file, CancellationToken ct = default)
    {
        try
        {
            using var newMemoryStream = file.OpenStream();
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = newMemoryStream,
                Key = fileName,
                BucketName = containerName,
                ContentType = file.ContentType,
            };

            using var fileTransferUtility = new TransferUtility(client);
            await fileTransferUtility.UploadAsync(uploadRequest, ct);

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
        catch (OperationCanceledException)
        {
            logger.Warning("Upload of file {FileName} was canceled", fileName);
            throw;
        }
        catch (Exception ex)
        {
            logger.Fatal(ex, "Unexpected error while uploading file {FileName}", fileName);
            throw;
        }

        return false;
    }

    /// <summary>
    /// Download file.
    /// </summary>
    /// <param name="fileName">File name.</param>
    /// <param name="ct">Cancellation token (optional).</param>
    /// <returns>Downloaded file data. Null otherwise.</returns>
    public async Task<FileData> DownloadFile(string fileName, CancellationToken ct = default)
    {
        MemoryStream ms = null;
        var fileData = new FileData();

        try
        {
            var request = new GetObjectRequest
            {
                BucketName = containerName,
                Key = fileName,
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
                logger.Error("Error: Document {@docName} not found", fileName);
                return null;
            }

            fileData.File = ms.ToArray();
            return fileData;
        }
        catch (AmazonS3Exception ex)
        {
            logger.Error(ex, "AWS S3 error while uploading file {FileName}", fileName);
        }
        catch (IOException ex)
        {
            logger.Error(ex, "I/O error while uploading file {FileName}", fileName);
        }
        catch (OperationCanceledException)
        {
            logger.Warning("Upload of file {FileName} was canceled", fileName);
            throw;
        }
        catch (Exception ex)
        {
            logger.Fatal(ex, "Unexpected error while uploading file {FileName}", fileName);
            throw;
        }

        return null;
    }

    /// <summary>
    /// Initialize AWS client.
    /// </summary>
    /// <returns>AWS client.</returns>
    private static AmazonS3Client InitClient()
    {
        var accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY");
        var secretkey = Environment.GetEnvironmentVariable("AWS_SECRET_KEY");

        var config = new AmazonS3Config
        {
            RegionEndpoint = RegionEndpoint.GetBySystemName(Environment.GetEnvironmentVariable("AWS_REGION")),
            ServiceURL = Environment.GetEnvironmentVariable("S3_ENDPOINT_URL"),
            UseHttp = true,
            ForcePathStyle = true,
        };

        return new(accessKey, secretkey, config);
    }
}
