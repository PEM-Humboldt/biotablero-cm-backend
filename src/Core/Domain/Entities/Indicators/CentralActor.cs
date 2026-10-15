namespace IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Central Actor entity.
/// </summary>
public class CentralActor : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Category name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Indicator Value relationship.
    /// </summary>
    public ICollection<IndicatorValue>? IndicatorValues { get; init; }
}
