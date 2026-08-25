namespace IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;

using System.Diagnostics.CodeAnalysis;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Territory Story Video dto.
/// </summary>
[method: SetsRequiredMembers]
public class TerritoryStoryVideoDto() : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Territory Story identifier.
    /// </summary>
    public int? TerritoryStoryId { get; set; }

    /// <summary>
    /// File URL.
    /// </summary>
    public required string FileUrl { get; set; } = string.Empty;
}
