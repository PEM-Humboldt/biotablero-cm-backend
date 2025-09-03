namespace IAVH.BioTablero.CM.Infrastructure.Integrations.Iam;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Interfaces.ExternalServices;

/// <summary>
/// Identity and Access Management service.
/// </summary>
public class IamService : IIamService
{
    private readonly HttpClient httpClient;
    private readonly string baseUrl;
    private readonly string baseUrlAdmin;
    private readonly string clientId;
    private readonly string clientSecret;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="httpClient">HTTP Client.</param>
    public IamService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
        baseUrl = $"{Environment.GetEnvironmentVariable("KC_BASE_URL")}/realms/{Environment.GetEnvironmentVariable("KC_REALM")}";
        baseUrlAdmin = $"{Environment.GetEnvironmentVariable("KC_BASE_URL")}/admin/realms/{Environment.GetEnvironmentVariable("KC_REALM")}";
        clientId = Environment.GetEnvironmentVariable("KC_CLIENT_BACKEND");
        clientSecret = Environment.GetEnvironmentVariable("KC_CLIENT_BACKEND_PASS");
    }

    /// <summary>
    /// Check if user exists.
    /// </summary>
    /// <param name="username">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if user exists. False otherwise.</returns>
    public async Task<bool> UserExistsAsync(string username, CancellationToken ct = default)
    {
        var token = await GetAdminTokenAsync(ct);

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var url = new Uri($"{baseUrlAdmin}/users?username={Uri.EscapeDataString(username)}");

        var response = await httpClient.GetAsync(url, ct);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(ct);
        var users = JsonSerializer.Deserialize<List<JsonElement>>(content);

        return users != null && users.Count > 0;
    }

    /// <summary>
    /// Get IAM admin token.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>JWT value.</returns>
    private async Task<string> GetAdminTokenAsync(CancellationToken ct)
    {
        var tokenUrl = new Uri($"{baseUrl}/protocol/openid-connect/token");

        using var content = new FormUrlEncodedContent(
        [
            new KeyValuePair<string, string>("client_id", clientId),
            new KeyValuePair<string, string>("client_secret", clientSecret),
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
        ]);

        var response = await httpClient.PostAsync(tokenUrl, content, ct);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(ct);
        var doc = JsonDocument.Parse(json);
        return doc.RootElement.GetProperty("access_token").GetString()!;
    }
}
