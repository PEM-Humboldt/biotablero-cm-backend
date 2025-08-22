namespace IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.Domain.Entities;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Initiative Join Request Status entity.
/// </summary>
public class InitiativeJoinRequestStatus : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Initiative Join Request Status name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Initiative Join request relationship.
    /// </summary>
    public ICollection<InitiativeJoinRequest> InitiativeJoinRequests { get; init; }
}
