namespace IAVH.BioTablero.CM.Application.DTOs.Indicators;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Indicator Value dto.
/// </summary>
public class IndicatorValueDto : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Date.
    /// </summary>
    public IndicatorDateDto Date { get; set; }

    /// <summary>
    /// Date end.
    /// </summary>
    public IndicatorDateDto DateEnd { get; set; }

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
    public MeasureUnitDto MeasureUnit { get; set; }
}
