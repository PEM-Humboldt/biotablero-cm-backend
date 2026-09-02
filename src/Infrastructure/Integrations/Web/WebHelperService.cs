namespace IAVH.BioTablero.CM.Infrastructure.Integrations.Web;

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices.Web;

/// <summary>
/// Web Helper service.
/// </summary>
/// <param name="httpClient">HTTP Client.</param>
public class WebHelperService(HttpClient httpClient) : IWebHelperService
{
    private readonly HttpClient httpClient = httpClient;

    /// <inheritdoc/>
    public async Task<bool> LinkExistsAsync(string url, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return false;
        }

        try
        {
            var response = await httpClient.GetAsync(url, ct);
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
