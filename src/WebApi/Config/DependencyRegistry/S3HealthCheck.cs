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
/// <param name="client">AWS S3 client.</param>
public class S3HealthCheck(IAmazonS3 client) : IHealthCheck
{
    private readonly IAmazonS3 client = client;
    private readonly string bucketName = Environment.GetEnvironmentVariable("S3_BUCKET_NAME");

    /// <inheritdoc/>
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new HeadBucketRequest
            {
                BucketName = bucketName,
            };

            await client.HeadBucketAsync(request, cancellationToken);

            return HealthCheckResult.Healthy();
        }
        catch (AmazonS3Exception ex)
        {
            return HealthCheckResult.Unhealthy("S3 bucket unavailable", ex);
        }
    }
}
