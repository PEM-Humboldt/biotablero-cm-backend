namespace IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;

using System;
using System.Diagnostics.CodeAnalysis;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Territory Story Image dto.
/// </summary>
[method: SetsRequiredMembers]
public class TerritoryStoryImageDto() : IDto
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
    public required Uri FileUrl { get; set; } = new(string.Empty);

    /// <summary>
    /// Entity description.
    /// </summary>
    public required string Description { get; set; } = string.Empty;

    /// <summary>
    /// Featured Content flag.
    /// </summary>
    public bool FeaturedContent { get; set; }
}
