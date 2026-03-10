namespace IAVH.BioTablero.CM.WebApi.Config.DependencyRegistry;

using System;
using System.Threading;
using System.Threading.Tasks;

using Amazon.S3;
using Amazon.S3.Model;

using Microsoft.Extensions.Diagnostics.HealthChecks;

/// <summary>
/// Custom AWS S3 Healht check.
/// </summary>
public class S3HealthCheck : IHealthCheck
{
    private readonly IAmazonS3 client;
    private readonly string bucketName = Environment.GetEnvironmentVariable("S3_BUCKET_NAME");

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="client">AWS S3 client.</param>
    public S3HealthCheck(IAmazonS3 client)
    {
        this.client = client;
    }

    /// <inheritdoc/>
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await client.ListObjectsV2Async(
                new ListObjectsV2Request
                {
                    BucketName = bucketName,
                    MaxKeys = 1,
                },
                cancellationToken);

            return HealthCheckResult.Healthy("S3 reachable");
        }
        catch (AmazonS3Exception ex)
        {
            return HealthCheckResult.Unhealthy("S3 unavailable", ex);
        }
    }
}