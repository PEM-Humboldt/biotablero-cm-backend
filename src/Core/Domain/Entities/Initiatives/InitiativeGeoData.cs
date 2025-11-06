namespace IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Initiative geo data.
/// </summary>
public class InitiativeGeoData
{
    /// <summary>
    /// Initiative identifier.
    /// </summary>
    public int InitiativeId { get; set; }

    /// <summary>
    /// Initiative name.
    /// </summary>
    public string InitiativeName { get; set; } = string.Empty;

    /// <summary>
    /// Initiative coordinates [longitude, latitude].
    /// </summary>
    public double[] Coordinate { get; set; } = [];
}
