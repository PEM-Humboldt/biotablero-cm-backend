namespace IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Topic Version entity.
/// </summary>
public class TopicVersion : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Topic identifier.
    /// </summary>
    public int TopicId { get; set; }

    /// <summary>
    /// Topic name.
    /// </summary>
    public string? TopicName { get; set; }

    /// <summary>
    /// Creation date.
    /// </summary>
    public DateTimeOffset CreationDate { get; set; }

    /// <summary>
    /// Version number.
    /// </summary>
    public int Version { get; set; }

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
    /// Indicator Topic identifier.
    /// </summary>
    public int? IndicatorTopicId { get; set; }

    /// <summary>
    /// Topic relationship.
    /// </summary>
    public Topic? Topic { get; set; }

    /// <summary>
    /// Topic Version Map relationship.
    /// </summary>
    public ICollection<TopicVersionMap>? Maps { get; init; }

    /// <summary>
    /// Topic Group relationship.
    /// </summary>
    public ICollection<TopicGroup>? Groups { get; init; }
}
