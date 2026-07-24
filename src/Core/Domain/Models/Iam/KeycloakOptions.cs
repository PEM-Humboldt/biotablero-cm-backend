namespace IAVH.BioTablero.CM.Core.Domain.Models.Iam;

/// <summary>
/// Keycloak connection options.
/// </summary>
public class KeycloakOptions
{
    /// <summary>
    /// Base URL.
    /// </summary>
    public string BaseUrl { get; set; }

    /// <summary>
    /// Keycloak realm.
    /// </summary>
    public string Realm { get; set; }

    /// <summary>
    /// Client identifier.
    /// </summary>
    public string ClientId { get; set; }

    /// <summary>
    /// Client secret.
    /// </summary>
    public string ClientSecret { get; set; }
}
