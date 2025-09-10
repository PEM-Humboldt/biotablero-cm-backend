namespace IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Tag Category entity.
/// </summary>
public class TagCategory : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Tag Category name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Tags relationship.
    /// </summary>
    public ICollection<Tag> Tags { get; init; }
}
