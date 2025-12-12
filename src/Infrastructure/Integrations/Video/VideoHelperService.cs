namespace IAVH.BioTablero.CM.Infrastructure.Integrations.Video;

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;

/// <summary>
/// Video service.
/// </summary>
public class VideoHelperService : IVideoHelperService
{
    private readonly HttpClient httpClient;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="httpClient">HTTP Client.</param>
    public VideoHelperService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

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
        catch (InvalidOperationException)
        {
            return false;
        }
        catch (HttpRequestException)
        {
            return false;
        }
        catch (TaskCanceledException)
        {
            return false;
        }
        catch (UriFormatException)
        {
            return false;
        }
    }
}
