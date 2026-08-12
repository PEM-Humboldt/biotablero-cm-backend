namespace IAVH.BioTablero.CM.Infrastructure.Integrations.Iam.TokenProviders;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Models.Iam;

using Microsoft.Extensions.Caching.Memory;

/// <summary>
/// Base class for Keycloak token providers.
/// </summary>
/// <param name="httpClient">HTTP Client.</param>
/// <param name="cache">Local cache.</param>
public abstract class BaseKeycloakTokenProvider(
    HttpClient httpClient,
    IMemoryCache cache)
{
    private const string CacheKeyPrefix = "KEYCLOAK_TOKEN";
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> Locks = new();

    /// <summary>
    /// Get client token using specific options.
    /// </summary>
    /// <param name="options">Keycloak options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Client token.</returns>
    protected async Task<string?> GetTokenInternalAsync(KeycloakOptions options, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(options);

        string cacheKey = $"{CacheKeyPrefix}_{options.ClientId}_{options.Realm}";

        if (cache.TryGetValue(cacheKey, out string? token))
        {
            return token;
        }

        var clientLock = Locks.GetOrAdd(cacheKey, _ => new SemaphoreSlim(1, 1));
        await clientLock.WaitAsync(ct);

        try
        {
            if (cache.TryGetValue(cacheKey, out token))
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
                cacheKey,
                token,
                TimeSpan.FromSeconds(tokenResponse.ExpiresIn - 30));

            return token;
        }
        finally
        {
            clientLock.Release();
        }
    }
}
