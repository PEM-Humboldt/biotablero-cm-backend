namespace IAVH.BioTablero.CM.Application.DTOs.Indicators;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Observation Version dto.
/// </summary>
public class ObservationVersionDto : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Observation identifier.
    /// </summary>
    public int? ObservationId { get; set; }

    /// <summary>
    /// Creation date.
    /// </summary>
    public DateTimeOffset? CreationDate { get; set; }

    /// <summary>
    /// Version number.
    /// </summary>
    public int? Version { get; set; }

    /// <summary>
    /// Observation Version description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Observation Version methodology.
    /// </summary>
    public string? Methodology { get; set; }

    /// <summary>
    /// Observation Version interpretation.
    /// </summary>
    public string? Interpretation { get; set; }

    /// <summary>
    /// Observation Version considerations.
    /// </summary>
    public string? Considerations { get; set; }

    /// <summary>
    /// Observation Version autorship.
    /// </summary>
    public string? Authorship { get; set; }

    /// <summary>
    /// Observation Group relationship.
    /// </summary>
    public IEnumerable<ObservationGroupDto>? Groups { get; init; }
}
