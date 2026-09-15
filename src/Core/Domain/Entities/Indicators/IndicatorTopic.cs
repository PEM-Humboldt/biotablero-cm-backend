namespace IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Indicator Topic entity.
/// </summary>
public class IndicatorTopic : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Entity name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Indicator relationship.
    /// </summary>
    public ICollection<Observation>? Indicators { get; init; }
}
