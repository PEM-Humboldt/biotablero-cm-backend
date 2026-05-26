namespace IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using IAVH.BioTablero.CM.Core.Domain.Entities.Tags;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Indicator Tag entity.
/// </summary>
public class IndicatorTag : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Indicator identifier.
    /// </summary>
    public int IndicatorId { get; set; }

    /// <summary>
    /// Tag identifier.
    /// </summary>
    public int TagId { get; set; }

    /// <summary>
    /// Indicator relationship.
    /// </summary>
    public Indicator Indicator { get; set; }

    /// <summary>
    /// Tag relationship.
    /// </summary>
    public Tag Tag { get; set; }
}
