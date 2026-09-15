namespace IAVH.BioTablero.CM.Application.DTOs.Indicators;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Observation dto.
/// </summary>
[method: SetsRequiredMembers]
public class ObservationDto() : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Observation name.
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
    /// Observation Topic.
    /// </summary>
    public IndicatorTopicDto? Topic { get; set; }

    /// <summary>
    /// Observation Locations relationship.
    /// </summary>
    public IEnumerable<IndicatorLocationDto>? Locations { get; set; }

    /// <summary>
    /// Observation versions list.
    /// </summary>
    public List<IndicatorVersionDto>? Versions { get; set; }

    /// <summary>
    /// Tags relationship.
    /// </summary>
    public IEnumerable<IndicatorTagDto>? Tags { get; set; }
}
