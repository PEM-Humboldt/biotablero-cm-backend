namespace IAVH.BioTablero.CM.Application.DTOs.Indicators;

using IAVH.BioTablero.CM.Application.DTOs.Geo;
using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Topic Location dto.
/// </summary>
public class TopicLocationDto : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Topic identifier.
    /// </summary>
    public int? TopicId { get; set; }

    /// <summary>
    /// Location identifier.
    /// </summary>
    public int? LocationId { get; set; }

    /// <summary>
    /// Locality name.
    /// </summary>
    public string? Locality { get; set; }

    /// <summary>
    /// Location relationship.
    /// </summary>
    public LocationDto? Location { get; set; }
}
