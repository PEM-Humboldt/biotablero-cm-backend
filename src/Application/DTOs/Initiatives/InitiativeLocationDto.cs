namespace IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using IAVH.BioTablero.CM.Application.DTOs.Geo;
using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Initiative Location dto.
/// </summary>
public class InitiativeLocationDto : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Initiative identifier.
    /// </summary>
    public int? InitiativeId { get; set; }

    /// <summary>
    /// Location identifier.
    /// </summary>
    public int? LocationId { get; set; }

    /// <summary>
    /// Locality.
    /// </summary>
    public string Locality { get; set; }

    /// <summary>
    /// Location relationship.
    /// </summary>
    public LocationDto Location { get; set; }
}
