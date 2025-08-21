namespace IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Initiative Tag Category entity.
/// </summary>
public class InitiativeTagCategory : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Initiative Tag Category name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Initiative Tags relationship.
    /// </summary>
    public ICollection<InitiativeTag> InitiativeTags { get; init; }
}
