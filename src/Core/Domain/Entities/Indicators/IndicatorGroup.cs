namespace IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Indicator Group entity.
/// </summary>
public class IndicatorGroup : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Indicator Version identifier.
    /// </summary>
    public int IndicatorVersionId { get; set; }

    /// <summary>
    /// Category identifier.
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Indicator Version relationship.
    /// </summary>
    public IndicatorVersion IndicatorVersion { get; set; }

    /// <summary>
    /// Category relationship.
    /// </summary>
    public Category Category { get; set; }

    /// <summary>
    /// Indicator Value relationship.
    /// </summary>
    public ICollection<IndicatorValue> Values { get; init; }
}
