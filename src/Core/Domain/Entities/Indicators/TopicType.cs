namespace IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Topic Type entity.
/// </summary>
public class TopicType : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Entity name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Topic relationship.
    /// </summary>
    public ICollection<Topic>? Topics { get; init; }
}
