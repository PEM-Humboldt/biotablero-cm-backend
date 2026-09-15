namespace IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Observation Version entity.
/// </summary>
public class ObservationVersion : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Observation identifier.
    /// </summary>
    public int ObservationId { get; set; }

    /// <summary>
    /// Creation date.
    /// </summary>
    public DateTimeOffset CreationDate { get; set; }

    /// <summary>
    /// Version number.
    /// </summary>
    public int Version { get; set; }

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
    /// Indicator Topic identifier.
    /// </summary>
    public int? IndicatorTopicId { get; set; }

    /// <summary>
    /// Observation relationship.
    /// </summary>
    public Observation? Observation { get; set; }

    /// <summary>
    /// Observation Version Map relationship.
    /// </summary>
    public ICollection<IndicatorVersionMap>? Maps { get; init; }

    /// <summary>
    /// Observation Group relationship.
    /// </summary>
    public ICollection<IndicatorGroup>? Groups { get; init; }
}
