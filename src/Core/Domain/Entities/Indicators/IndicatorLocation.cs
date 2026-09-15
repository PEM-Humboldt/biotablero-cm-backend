namespace IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Observation Location entity.
/// </summary>
public class IndicatorLocation : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Observation identifier.
    /// </summary>
    public int ObservationId { get; set; }

    /// <summary>
    /// Location identifier.
    /// </summary>
    public int LocationId { get; set; }

    /// <summary>
    /// Locality name.
    /// </summary>
    public string? Locality { get; set; }

    /// <summary>
    /// Observation relationship.
    /// </summary>
    public Observation? Observation { get; set; }

    /// <summary>
    /// Location relationship.
    /// </summary>
    public Location? Location { get; set; }
}
