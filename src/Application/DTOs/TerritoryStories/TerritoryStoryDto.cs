namespace IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Territory Story dto.
/// </summary>
public class TerritoryStoryDto : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Initiative identifier.
    /// </summary>
    public int? InitiativeId { get; set; }

    /// <summary>
    /// Author User Name identifier.
    /// </summary>
    public string AuthorUserName { get; set; }

    /// <summary>
    /// Territory Story title.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Territory Story title.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Territory Story keywords.
    /// </summary>
    public string Keywords { get; set; }

    /// <summary>
    /// Entity creation date.
    /// </summary>
    public DateTimeOffset? CreationDate { get; set; }

    /// <summary>
    /// Restricted flag.
    /// </summary>
    public bool? Restricted { get; set; }

    /// <summary>
    /// Enabled flag.
    /// </summary>
    public bool? Enabled { get; set; }

    /// <summary>
    /// Featured Content flag.
    /// </summary>
    public bool? FeaturedContent { get; set; }

    /// <summary>
    /// Total of territory story likes.
    /// </summary>
    public int? Likes { get; set; }

    /// <summary>
    /// Like action flag for authenticated users.
    /// </summary>
    public bool? ILikedIt { get; set; }

    /// <summary>
    /// Territory Story Image relationship.
    /// </summary>
    public IEnumerable<TerritoryStoryImageDto> Images { get; set; }

    /// <summary>
    /// Territory Story Video relationship.
    /// </summary>
    public IEnumerable<TerritoryStoryVideoDto> Videos { get; set; }
}
