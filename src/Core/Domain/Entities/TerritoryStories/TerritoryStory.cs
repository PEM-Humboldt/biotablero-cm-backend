namespace IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.Domain.Entities;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Territory Story entity.
/// </summary>
public class TerritoryStory : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Initiative identifier.
    /// </summary>
    public int InitiativeId { get; set; }

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
    public DateTimeOffset CreationDate { get; set; }

    /// <summary>
    /// Restricted flag.
    /// </summary>
    public bool Restricted { get; set; }

    /// <summary>
    /// Enabled flag.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Featured Content flag.
    /// </summary>
    public bool FeaturedContent { get; set; }

    /// <summary>
    /// Total of territory story likes.
    /// </summary>
    public int TotalLikes => Likes?.Count ?? 0;

    /// <summary>
    /// Like action flag for authenticated users.
    /// </summary>
    public bool? ILikedIt { get; set; }

    /// <summary>
    /// Initiative relationship.
    /// </summary>
    public Initiative Initiative { get; set; }

    /// <summary>
    /// Territory Story Like relationship.
    /// </summary>
    public ICollection<TerritoryStoryLike> Likes { get; init; }

    /// <summary>
    /// Territory Story Image relationship.
    /// </summary>
    public ICollection<TerritoryStoryImage> Images { get; init; }

    /// <summary>
    /// Territory Story Video relationship.
    /// </summary>
    public ICollection<TerritoryStoryVideo> Videos { get; init; }
}
