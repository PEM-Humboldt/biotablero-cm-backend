namespace IAVH.BioTablero.CM.Application.DTOs.Indicators;

using System.Diagnostics.CodeAnalysis;

using IAVH.BioTablero.CM.Application.DTOs.Tags;
using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Indicator Tag dto.
/// </summary>
[method: SetsRequiredMembers]
public class IndicatorTagDto() : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int IndicatorTagId { get; set; }

    /// <summary>
    /// Entity Tag.
    /// </summary>
    public TagDto? Tag { get; set; } = new();
}
