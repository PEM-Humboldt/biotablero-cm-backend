namespace IAVH.BioTablero.CM.Core.Domain.Models.Iam;

/// <summary>
/// Keycloak token response.
/// </summary>
public class TokenResponse
{
    /// <summary>
    /// Access token.
    /// </summary>
    public string AccessToken { get; set; }

    /// <summary>
    /// Validity period.
    /// </summary>
    public int ExpiresIn { get; set; }
}
