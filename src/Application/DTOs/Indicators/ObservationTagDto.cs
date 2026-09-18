namespace IAVH.BioTablero.CM.Application.DTOs.Indicators;

using System.Diagnostics.CodeAnalysis;

using IAVH.BioTablero.CM.Application.DTOs.Tags;
using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Observation Tag dto.
/// </summary>
[method: SetsRequiredMembers]
public class ObservationTagDto() : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int ObservationTagId { get; set; }

    /// <summary>
    /// Entity Tag.
    /// </summary>
    public TagDto? Tag { get; set; } = new();
}
