namespace IAVH.BioTablero.CM.Core.Domain.Initiatives;

using IAVH.BioTablero.CM.Core.Domain.Entities;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Level entity
/// </summary>
public class InitiativeUserLevel : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Level name
    /// </summary>
    public string Name { get; set; }
}
