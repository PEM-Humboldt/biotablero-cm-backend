namespace IAVH.BioTablero.CM.Infrastructure.Integrations.Iam.TokenProviders;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices.Iam;
using IAVH.BioTablero.CM.Core.Domain.Models.Iam;

using Microsoft.Extensions.Caching.Memory;

/// <summary>
/// Keycloak token provider.
/// </summary>
public class KeycloakTokenProvider : IKeycloakTokenProvider
{
    private const string CacheKey = "KEYCLOAK_TOKEN";
    private readonly HttpClient httpClient;
    private readonly IMemoryCache cache;
    private readonly KeycloakOptions options;
    private static readonly SemaphoreSlim Lock = new(1, 1);

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="httpClient">HTTP Client.</param>
    /// <param name="cache">Local cache.</param>
    public KeycloakTokenProvider(
        HttpClient httpClient,
        IMemoryCache cache)
    {
        this.httpClient = httpClient;
        this.cache = cache;
        options = new()
        {
            BaseUrl = $"{Environment.GetEnvironmentVariable("KC_BASE_URL")}/realms/{Environment.GetEnvironmentVariable("KC_REALM")}",
            Realm = Environment.GetEnvironmentVariable("KC_REALM"),
            ClientId = Environment.GetEnvironmentVariable("KC_CLIENT_BACKEND"),
            ClientSecret = Environment.GetEnvironmentVariable("KC_CLIENT_BACKEND_PASS"),
        };
    }

    /// <inheritdoc/>
    public async Task<string> GetTokenAsync(CancellationToken ct = default)
    {
        if (cache.TryGetValue(CacheKey, out string token))
        {
            return token;
        }

        await Lock.WaitAsync(ct);

        try
        {
            if (cache.TryGetValue(CacheKey, out token))
            {
                return token;
            }

            var form = new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials",
                ["client_id"] = options.ClientId,
                ["client_secret"] = options.ClientSecret,
            };

            var response = await httpClient.PostAsync(
                $"{options.BaseUrl}/protocol/openid-connect/token",
                new FormUrlEncodedContent(form),
                ct);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync(ct);
            using var doc = JsonDocument.Parse(jsonResponse);
            token = doc.RootElement.GetProperty("access_token").GetString()!;

            var tokenResponse = new TokenResponse()
            {
                AccessToken = doc.RootElement.GetProperty("access_token").GetString()!,
                ExpiresIn = doc.RootElement.GetProperty("expires_in").GetInt32()!,
            };

            cache.Set(
                CacheKey,
                token,
                TimeSpan.FromSeconds(tokenResponse.ExpiresIn - 30));

            return token;
        }
        finally
        {
            Lock.Release();
        }
    }
}
