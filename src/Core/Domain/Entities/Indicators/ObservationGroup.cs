namespace IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Observation Group entity.
/// </summary>
public class ObservationGroup : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Observation Version identifier.
    /// </summary>
    public int ObservationVersionId { get; set; }

    /// <summary>
    /// Category identifier.
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Observation Version relationship.
    /// </summary>
    public ObservationVersion? ObservationVersion { get; set; }

    /// <summary>
    /// Category relationship.
    /// </summary>
    public Category? Category { get; set; }

    /// <summary>
    /// Indicator Value relationship.
    /// </summary>
    public ICollection<IndicatorValue>? Values { get; init; }
}
