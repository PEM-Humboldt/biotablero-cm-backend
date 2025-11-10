namespace IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;

using System;

using IAVH.BioTablero.CM.Core.Domain.Entities;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Territory Story Image entity.
/// </summary>
public class TerritoryStoryImage : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Territory Story identifier.
    /// </summary>
    public int TerritoryStoryId { get; set; }

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

    /// <summary>
    /// Territory Story relationship.
    /// </summary>
    public TerritoryStory TerritoryStory { get; set; }
}
