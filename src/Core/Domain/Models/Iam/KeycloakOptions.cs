namespace IAVH.BioTablero.CM.Core.Domain.Models.Iam;

/// <summary>
/// Keycloak connection options.
/// </summary>
public class KeycloakOptions
{
    /// <summary>
    /// Base URL.
    /// </summary>
    public required string BaseUrl { get; set; }

    /// <summary>
    /// Keycloak realm.
    /// </summary>
    public required string Realm { get; set; }

    /// <summary>
    /// Client identifier.
    /// </summary>
    public required string ClientId { get; set; }

    /// <summary>
    /// Client secret.
    /// </summary>
    public required string ClientSecret { get; set; }
}
