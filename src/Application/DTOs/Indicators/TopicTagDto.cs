namespace IAVH.BioTablero.CM.Application.DTOs.Indicators;

using System.Diagnostics.CodeAnalysis;

using IAVH.BioTablero.CM.Application.DTOs.Tags;
using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Topic Tag dto.
/// </summary>
[method: SetsRequiredMembers]
public class TopicTagDto() : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int TopicTagId { get; set; }

    /// <summary>
    /// Entity Tag.
    /// </summary>
    public TagDto? Tag { get; set; } = new();
}
