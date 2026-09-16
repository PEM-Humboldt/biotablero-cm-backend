namespace IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using System;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Indicator Value entity.
/// </summary>
public class IndicatorValue : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Observation Group identifier.
    /// </summary>
    public int ObservationGroupId { get; set; }

    /// <summary>
    /// Indicator Type identifier.
    /// </summary>
    public int IndicatorTypeId { get; set; }

    /// <summary>
    /// Date.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Date end.
    /// </summary>
    public DateTime? DateEnd { get; set; }

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
    /// Observation Group relationship.
    /// </summary>
    public ObservationGroup? Group { get; set; }

    /// <summary>
    /// Indicator Type relationship.
    /// </summary>
    public IndicatorType? IndicatorType { get; set; }
}
