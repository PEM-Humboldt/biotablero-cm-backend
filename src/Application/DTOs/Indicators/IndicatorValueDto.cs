namespace IAVH.BioTablero.CM.Application.DTOs.Indicators;

using System;
using System.Diagnostics.CodeAnalysis;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Indicator Value dto.
/// </summary>
[method: SetsRequiredMembers]
public class IndicatorValueDto() : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Date.
    /// </summary>
    public required IndicatorDateDto Date { get; set; } = new(DateTimeOffset.Now);

    /// <summary>
    /// Date end.
    /// </summary>
    public IndicatorDateDto? DateEnd { get; set; }

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
    /// Measure Unit relationship.
    /// </summary>
    public MeasureUnitDto? MeasureUnit { get; set; }
}
