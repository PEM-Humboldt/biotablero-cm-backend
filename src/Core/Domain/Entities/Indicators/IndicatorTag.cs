namespace IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using IAVH.BioTablero.CM.Core.Domain.Entities.Tags;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Observation Tag entity.
/// </summary>
public class IndicatorTag : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Observation identifier.
    /// </summary>
    public int ObservationId { get; set; }

    /// <summary>
    /// Tag identifier.
    /// </summary>
    public int TagId { get; set; }

    /// <summary>
    /// Observation relationship.
    /// </summary>
    public Observation? Observation { get; set; }

    /// <summary>
    /// Tag relationship.
    /// </summary>
    public Tag? Tag { get; set; }
}
