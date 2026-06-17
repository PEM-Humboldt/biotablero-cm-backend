namespace IAVH.BioTablero.CM.Application.DTOs.Indicators;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Indicator Type dto.
/// </summary>
public class IndicatorTypeDto : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Indicator Type name.
    /// </summary>
    public string Name { get; set; }
}
