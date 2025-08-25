namespace IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.Domain.Entities;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Join Request Status entity.
/// </summary>
public class JoinRequestStatus : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Join Request Status name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Join request relationship.
    /// </summary>
    public ICollection<JoinRequest> JoinRequests { get; init; }
}
