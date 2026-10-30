namespace IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using System.Collections.Generic;
using System.Linq;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Topic entity.
/// </summary>
public class Topic : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Topic name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Initiative identifier.
    /// </summary>
    public int InitiativeId { get; set; }

    /// <summary>
    /// Topic Type identifier.
    /// </summary>
    public int TopicTypeId { get; set; }

    /// <summary>
    /// Initiative relationship.
    /// </summary>
    public Initiative? Initiative { get; set; }

    /// <summary>
    /// Topic Type relationship.
    /// </summary>
    public TopicType? TopicType { get; set; }

    /// <summary>
    /// Last topic version.
    /// </summary>
    public int LastVersion => Versions?.Max(e => e.Version) ?? 0;

    /// <summary>
    /// Topic Tag relationship.
    /// </summary>
    public ICollection<TopicTag>? TopicTags { get; init; }

    /// <summary>
    /// Topic Location relationship.
    /// </summary>
    public ICollection<TopicLocation>? TopicLocations { get; init; }

    /// <summary>
    /// Topic Version relationship.
    /// </summary>
    public ICollection<TopicVersion>? Versions { get; init; }
}
