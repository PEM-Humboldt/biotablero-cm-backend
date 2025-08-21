namespace IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.Domain.Entities;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Level entity.
/// </summary>
public class InitiativeUserLevel : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Level name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Initiative Users relationship.
    /// </summary>
    public ICollection<InitiativeUser> InitiativeUsers { get; init; }
}
