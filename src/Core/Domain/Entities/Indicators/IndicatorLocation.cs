namespace IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Indicator Location entity.
/// </summary>
public class IndicatorLocation : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Indicator identifier.
    /// </summary>
    public int IndicatorId { get; set; }

    /// <summary>
    /// Location identifier.
    /// </summary>
    public int LocationId { get; set; }

    /// <summary>
    /// Locality name.
    /// </summary>
    public string Locality { get; set; }

    /// <summary>
    /// Indicator relationship.
    /// </summary>
    public Indicator Indicator { get; set; }

    /// <summary>
    /// Location relationship.
    /// </summary>
    public Location Location { get; set; }
}
