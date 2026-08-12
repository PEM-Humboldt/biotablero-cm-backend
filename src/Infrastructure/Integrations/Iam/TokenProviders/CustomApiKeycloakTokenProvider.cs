namespace IAVH.BioTablero.CM.Infrastructure.Integrations.Iam.TokenProviders;

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices.Iam;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Models.Iam;

using Microsoft.Extensions.Caching.Memory;

/// <summary>
/// Keycloak token provider for Custom API.
/// </summary>
public class CustomApiKeycloakTokenProvider : BaseKeycloakTokenProvider, ICustomApiKeycloakTokenProvider
{
    private readonly KeycloakOptions options;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="httpClient">HTTP Client.</param>
    /// <param name="cache">Local cache.</param>
    public CustomApiKeycloakTokenProvider(
        HttpClient httpClient,
        IMemoryCache cache)
        : base(httpClient, cache)
    {
        options = new()
        {
            BaseUrl = $"{EnvUtils.GetRequiredEnv("KC_BASE_URL")}/realms/{EnvUtils.GetRequiredEnv("KC_REALM")}",
            Realm = EnvUtils.GetRequiredEnv("KC_REALM"),
            ClientId = EnvUtils.GetRequiredEnv("KC_CLIENT_CUSTOM_API"),
            ClientSecret = EnvUtils.GetRequiredEnv("KC_CLIENT_CUSTOM_API_PASS"),
        };
    }

    /// <inheritdoc/>
    public async Task<string?> GetTokenAsync(CancellationToken ct = default) => await GetTokenInternalAsync(options, ct);
}
