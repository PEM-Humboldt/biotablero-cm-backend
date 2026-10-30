namespace IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using IAVH.BioTablero.CM.Core.Domain.Entities.Tags;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Topic Tag entity.
/// </summary>
public class TopicTag : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Topic identifier.
    /// </summary>
    public int TopicId { get; set; }

    /// <summary>
    /// Tag identifier.
    /// </summary>
    public int TagId { get; set; }

    /// <summary>
    /// Topic relationship.
    /// </summary>
    public Topic? Topic { get; set; }

    /// <summary>
    /// Tag relationship.
    /// </summary>
    public Tag? Tag { get; set; }
}
