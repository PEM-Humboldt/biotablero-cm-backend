namespace IAVH.BioTablero.CM.Infrastructure.Integrations.Iam;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Core.Domain.Models.Iam;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.IamEnums;

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
    private readonly JsonSerializerOptions jsonSerializerOptions;

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

        jsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
        };
    }

    /// <inheritdoc/>
    public async Task<bool> UserExistsAsync(string username, CancellationToken ct = default)
    {
        var user = await GetUserData(UserVariable.Username, username, ct);
        return user != null;
    }

    /// <inheritdoc/>
    public async Task<ExternalUser> GetUserDataAsync(string username, CancellationToken ct = default) => await GetUserData(UserVariable.Username, username, ct);

    /// <inheritdoc/>
    public async Task<IEnumerable<ExternalUser>> GetUsersDataAsync(string[] usernames, CancellationToken ct = default)
    {
        var results = new List<ExternalUser>();
        var userTasks = usernames.Select(async username =>
        {
            var userData = await GetUserData(UserVariable.Username, username, ct);

            if (userData != null)
            {
                results.Add(userData);
            }
        });

        await Task.WhenAll(userTasks);

        return results;
    }

    /// <inheritdoc/>
    public async Task<ExternalUser> GetUserDataByEmailAsync(string email, CancellationToken ct = default) => await GetUserData(UserVariable.Email, email, ct);

    /// <inheritdoc/>
    public async Task<IEnumerable<ExternalUser>> GetUsersDataByEmailsAsync(string[] emails, CancellationToken ct = default)
    {
        var results = new List<ExternalUser>();
        var userTasks = emails.Select(async username =>
        {
            var userData = await GetUserData(UserVariable.Email, username, ct);

            if (userData != null)
            {
                results.Add(userData);
            }
        });

        await Task.WhenAll(userTasks);

        return results;
    }

    /// <summary>
    /// Get user data.
    /// </summary>
    /// <param name="userVariableName">User variable name.</param>
    /// <param name="userVariableValue">User variable value.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>User data.</returns>
    private async Task<ExternalUser> GetUserData(UserVariable userVariableName, string userVariableValue, CancellationToken ct = default)
    {
        var token = await GetAdminTokenAsync(ct);

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var url = new Uri($"{baseUrlAdmin}/users?exact=true&{userVariableName.ToString().ToLowerInvariant()}={Uri.EscapeDataString(userVariableValue)}");

        var response = await httpClient.GetAsync(url, ct);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(ct);

        var users = JsonSerializer.Deserialize<List<ExternalUser>>(content, jsonSerializerOptions);

        return users.FirstOrDefault();
    }

    /// <summary>
    /// Get IAM admin token.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>JWT value.</returns>
    private async Task<string> GetAdminTokenAsync(CancellationToken ct = default)
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
