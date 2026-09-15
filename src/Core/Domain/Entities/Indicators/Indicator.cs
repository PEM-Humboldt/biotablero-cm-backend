namespace IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Indicator entity.
/// </summary>
public class Indicator : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Indicator name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Initiative identifier.
    /// </summary>
    public int InitiativeId { get; set; }

    /// <summary>
    /// Indicator Topic identifier.
    /// </summary>
    public int IndicatorTopicId { get; set; }

    /// <summary>
    /// Initiative relationship.
    /// </summary>
    public Initiative? Initiative { get; set; }

    /// <summary>
    /// Indicator Topic relationship.
    /// </summary>
    public IndicatorTopic? Type { get; set; }

    /// <summary>
    /// Indicator Tag relationship.
    /// </summary>
    public ICollection<IndicatorTag>? IndicatorTags { get; init; }

    /// <summary>
    /// Indicator Location relationship.
    /// </summary>
    public ICollection<IndicatorLocation>? IndicatorLocations { get; init; }

    /// <summary>
    /// Indicator Version relationship.
    /// </summary>
    public ICollection<IndicatorVersion>? Versions { get; init; }
}
