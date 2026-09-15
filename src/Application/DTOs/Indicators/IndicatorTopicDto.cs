namespace IAVH.BioTablero.CM.Application.DTOs.Indicators;

using System.Diagnostics.CodeAnalysis;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Indicator Topic dto.
/// </summary>
[method: SetsRequiredMembers]
public class IndicatorTopicDto() : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Indicator Topic name.
    /// </summary>
    public required string Name { get; set; } = string.Empty;
}
