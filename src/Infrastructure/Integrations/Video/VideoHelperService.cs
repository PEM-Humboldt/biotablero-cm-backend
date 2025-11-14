namespace IAVH.BioTablero.CM.Infrastructure.Integrations.Video;

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

    /// <summary>
    /// Check if video exists.
    /// </summary>
    /// <param name="videoUrl">Video URL.</param>
    /// <param name="ct">Cancellation token (optional).</param>
    /// <returns>True if video exists. False otherwise.</returns>
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
        catch
        {
            return false;
        }
    }
}
