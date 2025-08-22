namespace IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using System;

using IAVH.BioTablero.CM.Core.Domain.Entities;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Initiative Join Request entity.
/// </summary>
public class InitiativeJoinRequest : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Initiative Join Request user name.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Initiative Join Request reviewer user name.
    /// </summary>
    public string ReviewerUserName { get; set; }

    /// <summary>
    /// Initiative Join Request creation date.
    /// </summary>
    public DateTime CreationDate { get; set; }

    /// <summary>
    /// Initiative Join Request creation date.
    /// </summary>
    public DateTime ResponseDate { get; set; }

    /// <summary>
    /// Initiative identifier.
    /// </summary>
    public int InitiativeId { get; set; }

    /// <summary>
    /// Initiative Join Request Status identifier.
    /// </summary>
    public int StatusId { get; set; }

    /// <summary>
    /// Initiative relationship.
    /// </summary>
    public Initiative Initiative { get; set; }

    /// <summary>
    /// Initiative Join request Status relationship.
    /// </summary>
    public InitiativeJoinRequestStatus Status { get; set; }
}
