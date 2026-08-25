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
    /// Item identifier.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Indicator Type name.
    /// </summary>
    public required string Name { get; set; } = string.Empty;
}
