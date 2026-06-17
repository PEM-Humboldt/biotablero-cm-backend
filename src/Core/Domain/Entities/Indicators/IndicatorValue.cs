namespace IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using System;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Indicator Value entity.
/// </summary>
public class IndicatorValue : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Indicator Group identifier.
    /// </summary>
    public int IndicatorGroupId { get; set; }

    /// <summary>
    /// Measure Unit identifier.
    /// </summary>
    public int MeasureUnitId { get; set; }

    /// <summary>
    /// Date.
    /// </summary>
    public DateTimeOffset Date { get; set; }

    /// <summary>
    /// Date end.
    /// </summary>
    public DateTimeOffset? DateEnd { get; set; }

    /// <summary>
    /// Indicator Value.
    /// </summary>
    public float Value { get; set; }

    /// <summary>
    /// Indicator Value upper limit.
    /// </summary>
    public float? UpperLimit { get; set; }

    /// <summary>
    /// Indicator Value lower limit.
    /// </summary>
    public float? LowerLimit { get; set; }

    /// <summary>
    /// Indicator Group relationship.
    /// </summary>
    public IndicatorGroup Group { get; set; }

    /// <summary>
    /// Measure Unit relationship.
    /// </summary>
    public MeasureUnit MeasureUnit { get; set; }
}
