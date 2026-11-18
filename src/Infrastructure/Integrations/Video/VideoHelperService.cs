namespace IAVH.BioTablero.CM.Infrastructure.Integrations.Video;

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices.Video;

/// <summary>
/// Video Helper service.
/// </summary>
/// <param name="httpClient">HTTP Client.</param>
public class VideoHelperService(HttpClient httpClient) : IVideoHelperService
{
    private readonly HttpClient httpClient = httpClient;

    /// <inheritdoc/>
    public async Task<bool> VideoExistsAsync(string videoUrl, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(videoUrl))
        {
            return false;
        }

        string oembedUrl = $"https://www.youtube.com/oembed?url={videoUrl}&format=json";

        try
        {
            var response = await httpClient.GetAsync(oembedUrl, ct);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex) when (
            ex is InvalidOperationException or
            HttpRequestException or
            TaskCanceledException or
            UriFormatException)
        {
            return false;
        }
    }
}
