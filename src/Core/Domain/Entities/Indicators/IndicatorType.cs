namespace IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Indicator Type entity.
/// </summary>
public class IndicatorType : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Indicator Type name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Measure Unit representation.
    /// </summary>
    public string? Representation { get; set; }

    /// <summary>
    /// Indicator Value relationship.
    /// </summary>
    public ICollection<IndicatorValue>? IndicatorValues { get; init; }
}
