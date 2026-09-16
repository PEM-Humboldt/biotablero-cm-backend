namespace IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Observation Version Map entity.
/// </summary>
public class ObservationVersionMap : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Observation Version identifier.
    /// </summary>
    public int ObservationVersionId { get; set; }

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
    /// Observation Version relationship.
    /// </summary>
    public ObservationVersion? ObservationVersion { get; set; }

    /// <summary>
    /// Map Legend relationship.
    /// </summary>
    public ICollection<MapLegend>? Legends { get; init; }
}
