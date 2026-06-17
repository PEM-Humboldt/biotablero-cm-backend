namespace IAVH.BioTablero.CM.Application.DTOs.Indicators;

using IAVH.BioTablero.CM.Application.DTOs.Geo;
using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Indicator Location dto.
/// </summary>
public class IndicatorLocationDto : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Indicator identifier.
    /// </summary>
    public int? IndicatorId { get; set; }

    /// <summary>
    /// Location identifier.
    /// </summary>
    public int? LocationId { get; set; }

    /// <summary>
    /// Locality name.
    /// </summary>
    public string Locality { get; set; }

    /// <summary>
    /// Location relationship.
    /// </summary>
    public LocationDto Location { get; set; }
}
