namespace IAVH.BioTablero.CM.Application.DTOs.Resources;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Resource dto.
/// </summary>
[method: SetsRequiredMembers]
public class ResourceDto() : IDto
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
    public required string AuthorUserName { get; set; } = string.Empty;

    /// <summary>
    /// Entity name.
    /// </summary>
    public required string Name { get; set; } = string.Empty;

    /// <summary>
    /// Entity description.
    /// </summary>
    public required string Description { get; set; } = string.Empty;

    /// <summary>
    /// Entity creation date.
    /// </summary>
    public DateTimeOffset? CreationDate { get; set; }

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
    public int? Likes { get; set; }

    /// <summary>
    /// Like action flag for authenticated users.
    /// </summary>
    public bool? ILikedIt { get; set; }

    /// <summary>
    /// Total of resource files.
    /// </summary>
    public int? TotalFiles { get; set; }

    /// <summary>
    /// Total of resource links.
    /// </summary>
    public int? TotalLinks { get; set; }

    /// <summary>
    /// Resource Type relationship.
    /// </summary>
    public ResourceTypeDto? ResourceType { get; set; }

    /// <summary>
    /// Initiative relationship.
    /// </summary>
    public InitiativeDto? Initiative { get; set; }

    /// <summary>
    /// Resource File relationship.
    /// </summary>
    public IEnumerable<ResourceFileDto>? Files { get; init; }

    /// <summary>
    /// Resource Link relationship.
    /// </summary>
    public IEnumerable<ResourceLinkDto>? Links { get; init; }

    /// <summary>
    /// Tags relationship.
    /// </summary>
    public IEnumerable<ResourceTagDto>? Tags { get; init; }
}
