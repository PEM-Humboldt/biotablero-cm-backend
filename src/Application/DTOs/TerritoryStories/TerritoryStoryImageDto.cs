namespace IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;

using System;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Territory Story Image dto.
/// </summary>
public class TerritoryStoryImageDto : IDto
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
    public Uri FileUrl { get; set; }

    /// <summary>
    /// Entity description.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Featured Content flag.
    /// </summary>
    public bool FeaturedContent { get; set; }
}
