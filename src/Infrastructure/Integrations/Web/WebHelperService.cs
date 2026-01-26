namespace IAVH.BioTablero.CM.Infrastructure.Integrations.Web;

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;

/// <summary>
/// Web Helper service.
/// </summary>
public class WebHelperService : IWebHelperService
{
    private readonly HttpClient httpClient;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="httpClient">HTTP Client.</param>
    public WebHelperService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

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
