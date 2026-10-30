namespace IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Topic Group entity.
/// </summary>
public class TopicGroup : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Topic Version identifier.
    /// </summary>
    public int TopicVersionId { get; set; }

    /// <summary>
    /// Category identifier.
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Topic Version relationship.
    /// </summary>
    public TopicVersion? TopicVersion { get; set; }

    /// <summary>
    /// Category relationship.
    /// </summary>
    public Category? Category { get; set; }

    /// <summary>
    /// Indicator Value relationship.
    /// </summary>
    public ICollection<IndicatorValue>? Values { get; init; }
}
