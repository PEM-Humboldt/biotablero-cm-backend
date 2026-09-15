namespace IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Observation entity.
/// </summary>
public class Observation : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Observation name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Initiative identifier.
    /// </summary>
    public int InitiativeId { get; set; }

    /// <summary>
    /// Observation Topic identifier.
    /// </summary>
    public int IndicatorTopicId { get; set; }

    /// <summary>
    /// Initiative relationship.
    /// </summary>
    public Initiative? Initiative { get; set; }

    /// <summary>
    /// Observation Topic relationship.
    /// </summary>
    public IndicatorTopic? Topic { get; set; }

    /// <summary>
    /// Observation Tag relationship.
    /// </summary>
    public ICollection<ObservationTag>? ObservationTags { get; init; }

    /// <summary>
    /// Observation Location relationship.
    /// </summary>
    public ICollection<ObservationLocation>? ObservationLocations { get; init; }

    /// <summary>
    /// Observation Version relationship.
    /// </summary>
    public ICollection<IndicatorVersion>? Versions { get; init; }
}
