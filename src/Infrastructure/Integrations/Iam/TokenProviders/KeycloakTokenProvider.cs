namespace IAVH.BioTablero.CM.Infrastructure.Integrations.Iam.TokenProviders;

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices.Iam;
using IAVH.BioTablero.CM.Core.Domain.Models.Iam;

using Microsoft.Extensions.Caching.Memory;

/// <summary>
/// Keycloak token provider.
/// </summary>
public class KeycloakTokenProvider : BaseKeycloakTokenProvider, IKeycloakTokenProvider
{
    private readonly KeycloakOptions options;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="httpClient">HTTP Client.</param>
    /// <param name="cache">Local cache.</param>
    public KeycloakTokenProvider(
        HttpClient httpClient,
        IMemoryCache cache)
        : base(httpClient, cache)
    {
        options = new()
        {
            BaseUrl = $"{Environment.GetEnvironmentVariable("KC_BASE_URL")}/realms/{Environment.GetEnvironmentVariable("KC_REALM")}",
            Realm = Environment.GetEnvironmentVariable("KC_REALM"),
            ClientId = Environment.GetEnvironmentVariable("KC_CLIENT_BACKEND"),
            ClientSecret = Environment.GetEnvironmentVariable("KC_CLIENT_BACKEND_PASS"),
        };
    }

    /// <inheritdoc/>
    public async Task<string> GetTokenAsync(CancellationToken ct = default) => await GetTokenInternalAsync(options, ct);
}
