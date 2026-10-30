namespace IAVH.BioTablero.CM.Application.DTOs.Indicators;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Topic Version dto.
/// </summary>
public class TopicVersionDto : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Topic identifier.
    /// </summary>
    public int? TopicId { get; set; }

    /// <summary>
    /// Creation date.
    /// </summary>
    public DateTimeOffset? CreationDate { get; set; }

    /// <summary>
    /// Version number.
    /// </summary>
    public int? Version { get; set; }

    /// <summary>
    /// Topic Version description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Topic Version methodology.
    /// </summary>
    public string? Methodology { get; set; }

    /// <summary>
    /// Topic Version interpretation.
    /// </summary>
    public string? Interpretation { get; set; }

    /// <summary>
    /// Topic Version considerations.
    /// </summary>
    public string? Considerations { get; set; }

    /// <summary>
    /// Topic Version autorship.
    /// </summary>
    public string? Authorship { get; set; }

    /// <summary>
    /// Topic Group relationship.
    /// </summary>
    public IEnumerable<TopicGroupDto>? Groups { get; init; }
}
