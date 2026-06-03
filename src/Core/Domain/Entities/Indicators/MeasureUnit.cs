namespace IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Measure Unit entity.
/// </summary>
public class MeasureUnit : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Measure Unit name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Measure Unit representation.
    /// </summary>
    public string Representation { get; set; }

    /// <summary>
    /// Indicator Value relationship.
    /// </summary>
    public ICollection<IndicatorValue> IndicatorValues { get; init; }
}
