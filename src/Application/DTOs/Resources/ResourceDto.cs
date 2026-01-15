namespace IAVH.BioTablero.CM.Application.DTOs.Resources;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.DTOs.Tags;
using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Resource dto.
/// </summary>
public class ResourceDto : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Initiative identifier.
    /// </summary>
    public int InitiativeId { get; set; }

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
    public DateTime CreationDate { get; set; }

    /// <summary>
    /// Entity publication date.
    /// </summary>
    public DateTime PublicationDate { get; set; }

    /// <summary>
    /// Is draft flag.
    /// </summary>
    public bool IsDraft { get; set; }

    /// <summary>
    /// Total of territory story likes.
    /// </summary>
    public int? Likes { get; set; }

    /// <summary>
    /// Like action flag for authenticated users.
    /// </summary>
    public bool? ILikedIt { get; set; }

    /// <summary>
    /// Resource Type relationship.
    /// </summary>
    public ResourceTypeDto ResourceType { get; set; }

    /// <summary>
    /// Resource File relationship.
    /// </summary>
    public ICollection<ResourceFileDto> Files { get; init; }

    /// <summary>
    /// Resource Link relationship.
    /// </summary>
    public ICollection<ResourceLinkDto> Links { get; init; }

    /// <summary>
    /// Tags relationship.
    /// </summary>
    public IEnumerable<TagDto> Tags { get; init; }
}
