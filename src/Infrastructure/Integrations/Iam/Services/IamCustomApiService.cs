namespace IAVH.BioTablero.CM.Infrastructure.Integrations.Iam.Services;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices.Iam;
using IAVH.BioTablero.CM.Core.Domain.Models.Iam;

/// <summary>
/// Identity and Access Management service for custom API.
/// </summary>
/// <param name="httpClient">HTTP Client.</param>
/// <param name="tokenProvider">Keycloak token provider.</param>
public class IamCustomApiService(HttpClient httpClient,
ICustomApiKeycloakTokenProvider tokenProvider) : IIamCustomApiService
{
    /// <inheritdoc/>
    public async Task<List<ExternalUser>> GetUsersDataAsync(string query, CancellationToken ct = default)
    {
        var token = await tokenProvider.GetTokenAsync(ct);

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await httpClient.GetAsync(query, ct);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(ct);
        return MapKeycloakUsers(content);
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

        var valueArray = default(JsonElement);

        if (jsonDoc.RootElement.ValueKind != JsonValueKind.Object ||
            !(jsonDoc.RootElement.TryGetProperty("value", out valueArray) && valueArray.ValueKind == JsonValueKind.Array))
        {
            return users;
        }

        foreach (var element in valueArray.EnumerateArray())
        {
            var user = new ExternalUser
            {
                Id = element.TryGetProperty("id", out var id) && id.ValueKind == JsonValueKind.String && Guid.TryParse(id.GetString(), out var guidId) ? guidId : Guid.Empty,
                Username = element.TryGetProperty("username", out var username) && username.ValueKind == JsonValueKind.String ? username.GetString() : null,
                FirstName = element.TryGetProperty("firstName", out var firstName) && firstName.ValueKind == JsonValueKind.String ? firstName.GetString() : null,
                LastName = element.TryGetProperty("lastName", out var lastName) && lastName.ValueKind == JsonValueKind.String ? lastName.GetString() : null,
                Email = element.TryGetProperty("email", out var email) && email.ValueKind == JsonValueKind.String ? email.GetString() : null,
                EmailVerified = element.TryGetProperty("emailVerified", out var emailVerified) && emailVerified.ValueKind == JsonValueKind.True,
                Enabled = element.TryGetProperty("enabled", out var enabled) && enabled.ValueKind == JsonValueKind.True,
                Phone = element.TryGetProperty("phone", out var phone) && phone.ValueKind == JsonValueKind.String ? phone.GetString() : null,

                Picture = element.TryGetProperty("picture", out var picture) && picture.ValueKind == JsonValueKind.String ? picture.GetString() : null,
                Organization = element.TryGetProperty("organization", out var org) && org.ValueKind == JsonValueKind.String ? org.GetString() : null,
                SelfRecognition = element.TryGetProperty("selfRecognition", out var selfRec) && selfRec.ValueKind == JsonValueKind.String ? selfRec.GetString() : null,
                Gender = element.TryGetProperty("gender", out var gen) && gen.ValueKind == JsonValueKind.String ? gen.GetString() : null,
            };

            users.Add(user);
        }

        return users;
    }
}
