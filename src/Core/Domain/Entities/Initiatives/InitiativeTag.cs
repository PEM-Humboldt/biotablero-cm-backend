namespace IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Initiative Tag entity.
/// </summary>
public class InitiativeTag : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Initiative Tag name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Initiative Tag URL.
    /// </summary>
    public Uri Url { get; set; }

    /// <summary>
    /// Initiative Tag Category identifier.
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Tag Category relationship.
    /// </summary>
    public InitiativeTagCategory Category { get; set; }

    /// <summary>
    /// Initiative Tag Initiative relationship.
    /// </summary>
    public ICollection<InitiativeTagInitiative> InitiativeTagInitiatives { get; init; }
}
