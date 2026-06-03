namespace IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.Domain.Entities;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Join Invitation entity.
/// </summary>
public class JoinInvitation : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Initiative identifier.
    /// </summary>
    public int InitiativeId { get; set; }

    /// <summary>
    /// Creator user identifier.
    /// </summary>
    public string Creator { get; set; }

    /// <summary>
    /// Creator user full name.
    /// </summary>
    public string CreatorFullName { get; set; }

    /// <summary>
    /// Join Invitation message.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Join Invitation HTML message.
    /// </summary>
    public string HtmlMessage { get; set; }

    /// <summary>
    /// Join Invitation creation date.
    /// </summary>
    public DateTimeOffset CreationDate { get; set; }

    /// <summary>
    /// Initiative relationship.
    /// </summary>
    public Initiative Initiative { get; set; }

    /// <summary>
    /// Join Invitation Guests relationship.
    /// </summary>
    public ICollection<JoinInvitationGuest> Guests { get; init; }
}
