namespace IAVH.BioTablero.CM.Application.DTOs.Indicators;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Topic dto.
/// </summary>
[method: SetsRequiredMembers]
public class TopicDto() : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Topic name.
    /// </summary>
    public required string Name { get; set; } = string.Empty;

    /// <summary>
    /// Initiative identifier.
    /// </summary>
    public int? InitiativeId { get; set; }

    /// <summary>
    /// Initiative name.
    /// </summary>
    public string? InitiativeName { get; set; }

    /// <summary>
    /// Topic Topic.
    /// </summary>
    public TopicTypeDto? TopicType { get; set; }

    /// <summary>
    /// Topic Locations relationship.
    /// </summary>
    public IEnumerable<TopicLocationDto>? Locations { get; set; }

    /// <summary>
    /// Topic versions list.
    /// </summary>
    public List<TopicVersionDto>? Versions { get; set; }

    /// <summary>
    /// Tags relationship.
    /// </summary>
    public IEnumerable<TopicTagDto>? Tags { get; set; }
}
