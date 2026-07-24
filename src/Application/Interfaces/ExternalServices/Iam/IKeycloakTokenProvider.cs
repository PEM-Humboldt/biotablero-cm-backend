namespace IAVH.BioTablero.CM.Application.Interfaces.ExternalServices.Iam;

using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Keycloak token provider interface.
/// </summary>
public interface IKeycloakTokenProvider
{
    /// <summary>
    /// Get client token.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Client token.</returns>
    Task<string> GetTokenAsync(CancellationToken ct = default);
}
