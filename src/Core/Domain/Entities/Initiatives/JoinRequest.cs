namespace IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using System;

using IAVH.BioTablero.CM.Core.Domain.Entities;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Join Request entity.
/// </summary>
public class JoinRequest : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Join Request user name.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Join Request reviewer user name.
    /// </summary>
    public string ReviewerUserName { get; set; }

    /// <summary>
    /// Join Request creation date.
    /// </summary>
    public DateTimeOffset CreationDate { get; set; }

    /// <summary>
    /// Join Request response date.
    /// </summary>
    public DateTimeOffset? ResponseDate { get; set; }

    /// <summary>
    /// Initiative identifier.
    /// </summary>
    public int InitiativeId { get; set; }

    /// <summary>
    /// Level identifier.
    /// </summary>
    public int? LevelId { get; set; }

    /// <summary>
    /// Join Request Status identifier.
    /// </summary>
    public int StatusId { get; set; }

    /// <summary>
    /// Initiative relationship.
    /// </summary>
    public Initiative Initiative { get; set; }

    /// <summary>
    /// Level relationship.
    /// </summary>
    public InitiativeUserLevel Level { get; set; }

    /// <summary>
    /// Join request Status relationship.
    /// </summary>
    public JoinRequestStatus Status { get; set; }
}
