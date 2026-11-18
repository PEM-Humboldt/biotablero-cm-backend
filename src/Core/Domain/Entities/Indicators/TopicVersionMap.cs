namespace IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Topic Version Map entity.
/// </summary>
public class TopicVersionMap : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Topic Version identifier.
    /// </summary>
    public int TopicVersionId { get; set; }

    /// <summary>
    /// Map title.
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Map description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Map Image URL.
    /// </summary>
    public required string ImageUrl { get; set; }

    /// <summary>
    /// Topic Version relationship.
    /// </summary>
    public TopicVersion? TopicVersion { get; set; }

    /// <summary>
    /// Map Legend relationship.
    /// </summary>
    public ICollection<MapLegend>? Legends { get; init; }
}
