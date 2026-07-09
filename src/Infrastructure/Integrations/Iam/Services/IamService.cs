namespace IAVH.BioTablero.CM.Infrastructure.Integrations.Iam.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices.Iam;
using IAVH.BioTablero.CM.Core.Domain.Models.Iam;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.IamEnums;

/// <summary>
/// Identity and Access Management service.
/// </summary>
public class IamService : IIamService
{
    private readonly HttpClient httpClient;
    private readonly IKeycloakTokenProvider tokenProvider;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="httpClient">HTTP Client.</param>
    /// <param name="tokenProvider">Keycloak token provider.</param>
    public IamService(
        HttpClient httpClient,
        IKeycloakTokenProvider tokenProvider)
    {
        this.httpClient = httpClient;
        this.tokenProvider = tokenProvider;
    }

    /// <inheritdoc/>
    public async Task<bool> UserExistsAsync(string username, CancellationToken ct = default)
    {
        var user = await GetKeycloakUserDataAsync(UserVariable.Username, username, ct);
        return user != null;
    }

    /// <inheritdoc/>
    public async Task<ExternalUser> GetUserDataAsync(string username, CancellationToken ct = default) =>
        await GetKeycloakUserDataAsync(UserVariable.Username, username, ct);

    /// <inheritdoc/>
    public async Task<IEnumerable<ExternalUser>> GetUsersDataAsync(string[] usernames, CancellationToken ct = default)
    {
        var results = new List<ExternalUser>();
        var userTasks = usernames.Select(async username =>
        {
            var userData = await GetKeycloakUserDataAsync(UserVariable.Username, username, ct);

            if (userData != null)
            {
                results.Add(userData);
            }
        });

        await Task.WhenAll(userTasks);

        return results;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ExternalUser>> GetAllEnabledUsersDataAsync(CancellationToken ct = default)
    {
        var token = await tokenProvider.GetTokenAsync(ct);

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await httpClient.GetAsync($"users?enabled=true&emailVerified=true", ct);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(ct);

        return MapKeycloakUsers(content);
    }

    /// <summary>
    /// Get user data.
    /// </summary>
    /// <param name="userVariableName">User variable name.</param>
    /// <param name="userVariableValue">User variable value.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>User data.</returns>
    private async Task<ExternalUser> GetKeycloakUserDataAsync(UserVariable userVariableName, string userVariableValue, CancellationToken ct = default)
    {
        var token = await tokenProvider.GetTokenAsync(ct);

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await httpClient.GetAsync($"users?exact=true&{userVariableName.ToString().ToLowerInvariant()}={Uri.EscapeDataString(userVariableValue)}", ct);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(ct);

        var users = MapKeycloakUsers(content);

        return users.FirstOrDefault();
    }

    /// <summary>
    /// Maps Keycloak JSON string response to ExternalUser list.
    /// </summary>
    /// <param name="jsonContent">JSON content from Keycloak API.</param>
    /// <returns>List of external users.</returns>
    private static List<ExternalUser> MapKeycloakUsers(string jsonContent)
    {
        var users = new List<ExternalUser>();
        using var jsonDoc = JsonDocument.Parse(jsonContent);

        if (jsonDoc.RootElement.ValueKind != JsonValueKind.Array)
        {
            return users;
        }

        foreach (var element in jsonDoc.RootElement.EnumerateArray())
        {
            var user = new ExternalUser
            {
                Id = element.TryGetProperty("id", out var id) && id.ValueKind == JsonValueKind.String && Guid.TryParse(id.GetString(), out var guidId) ? guidId : Guid.Empty,
                Email = element.TryGetProperty("email", out var email) && email.ValueKind == JsonValueKind.String ? email.GetString() : null,
                EmailVerified = element.TryGetProperty("emailVerified", out var emailVerified) && emailVerified.ValueKind == JsonValueKind.True,
                Username = element.TryGetProperty("username", out var username) && username.ValueKind == JsonValueKind.String ? username.GetString() : null,
                FirstName = element.TryGetProperty("firstName", out var firstName) && firstName.ValueKind == JsonValueKind.String ? firstName.GetString() : null,
                LastName = element.TryGetProperty("lastName", out var lastName) && lastName.ValueKind == JsonValueKind.String ? lastName.GetString() : null,
                CreationDate = element.TryGetProperty("createdTimestamp", out var createdTs) && createdTs.ValueKind == JsonValueKind.Number ? DateTimeOffset.FromUnixTimeMilliseconds(createdTs.GetInt64()).UtcDateTime : null,
            };

            if (element.TryGetProperty("attributes", out var attributes) && attributes.ValueKind == JsonValueKind.Object)
            {
                if (attributes.TryGetProperty("phone", out var phone) && phone.ValueKind == JsonValueKind.Array && phone.GetArrayLength() > 0)
                {
                    user.Phone = phone[0].GetString();
                }

                if (attributes.TryGetProperty("picture", out var picture) && picture.ValueKind == JsonValueKind.Array && picture.GetArrayLength() > 0)
                {
                    user.Picture = picture[0].GetString();
                }

                if (attributes.TryGetProperty("organizacion", out var org) && org.ValueKind == JsonValueKind.Array && org.GetArrayLength() > 0)
                {
                    user.Organization = org[0].GetString();
                }

                if (attributes.TryGetProperty("autorreconocimiento", out var selfRec) && selfRec.ValueKind == JsonValueKind.Array && selfRec.GetArrayLength() > 0)
                {
                    user.SelfRecognition = selfRec[0].GetString();
                }

                if (attributes.TryGetProperty("genero", out var gen) && gen.ValueKind == JsonValueKind.Array && gen.GetArrayLength() > 0)
                {
                    user.Gender = gen[0].GetString();
                }
            }

            users.Add(user);
        }

        return users;
    }
}
