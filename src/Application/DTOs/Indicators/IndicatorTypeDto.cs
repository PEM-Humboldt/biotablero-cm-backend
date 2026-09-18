namespace IAVH.BioTablero.CM.Application.DTOs.Indicators;

using System.Diagnostics.CodeAnalysis;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Indicator Type dto.
/// </summary>
[method: SetsRequiredMembers]
public class IndicatorTypeDto() : IDto
{
    /// <summary>
    /// Indicator Type name.
    /// </summary>
    public required string Name { get; set; } = string.Empty;

    /// <summary>
    /// Measure Unit representation.
    /// </summary>
    public string? Representation { get; set; }
}
