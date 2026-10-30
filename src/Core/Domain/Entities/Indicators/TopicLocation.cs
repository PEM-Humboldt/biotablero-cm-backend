namespace IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Topic Location entity.
/// </summary>
public class TopicLocation : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Topic identifier.
    /// </summary>
    public int TopicId { get; set; }

    /// <summary>
    /// Location identifier.
    /// </summary>
    public int LocationId { get; set; }

    /// <summary>
    /// Locality name.
    /// </summary>
    public string? Locality { get; set; }

    /// <summary>
    /// Topic relationship.
    /// </summary>
    public Topic? Topic { get; set; }

    /// <summary>
    /// Location relationship.
    /// </summary>
    public Location? Location { get; set; }
}
