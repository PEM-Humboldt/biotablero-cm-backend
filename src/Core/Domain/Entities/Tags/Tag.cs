namespace IAVH.BioTablero.CM.Core.Domain.Entities.Tags;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Tag entity.
/// </summary>
public class Tag : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Tag name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Tag full name.
    /// </summary>
    public string? FullName { get; set; }

    /// <summary>
    /// Tag URL.
    /// </summary>
    public Uri? Url { get; set; }

    /// <summary>
    /// Tag Category identifier.
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Tag Category relationship.
    /// </summary>
    public TagCategory? Category { get; set; }

    /// <summary>
    /// Initiative Tag relationship.
    /// </summary>
    public ICollection<InitiativeTag>? TagInitiatives { get; init; }

    /// <summary>
    /// Resource Tag relationship.
    /// </summary>
    public ICollection<ResourceTag>? TagResources { get; init; }

    /// <summary>
    /// Topic Tag relationship.
    /// </summary>
    public ICollection<TopicTag>? TopicTags { get; init; }
}
