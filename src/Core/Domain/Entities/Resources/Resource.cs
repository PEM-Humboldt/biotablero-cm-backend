namespace IAVH.BioTablero.CM.Core.Domain.Entities.Resources;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.Domain.Entities;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Resource entity.
/// </summary>
public class Resource : BaseEntity<int>, IAggregateRoot
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
    /// Resource Type identifier.
    /// </summary>
    public int ResourceTypeId { get; set; }

    /// <summary>
    /// Entity name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Entity description.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Entity creation date.
    /// </summary>
    public DateTimeOffset CreationDate { get; set; }

    /// <summary>
    /// Entity publication date.
    /// </summary>
    public DateTimeOffset? PublicationDate { get; set; }

    /// <summary>
    /// Is draft flag.
    /// </summary>
    public bool IsDraft { get; set; }

    /// <summary>
    /// Total of resource likes.
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
    /// Resource Type relationship.
    /// </summary>
    public ResourceType ResourceType { get; set; }

    /// <summary>
    /// Resource Like relationship.
    /// </summary>
    public ICollection<ResourceLike> Likes { get; init; }

    /// <summary>
    /// Resource File relationship.
    /// </summary>
    public ICollection<ResourceFile> Files { get; init; }

    /// <summary>
    /// Resource Link relationship.
    /// </summary>
    public ICollection<ResourceLink> Links { get; init; }

    /// <summary>
    /// Resource Tag relationship.
    /// </summary>
    public ICollection<ResourceTag> ResourceTags { get; init; }
}
