namespace IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Tag entity.
/// </summary>
public class Tag : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Tag name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Tag URL.
    /// </summary>
    public Uri Url { get; set; }

    /// <summary>
    /// Tag Category identifier.
    /// </summary>
    public int TagCategoryId { get; set; }

    /// <summary>
    /// Tag Category relationship.
    /// </summary>
    public TagCategory TagCategory { get; set; }

    /// <summary>
    /// Tag Initiative relationship.
    /// </summary>
    public ICollection<InitiativeTag> TagInitiatives { get; init; }
}
