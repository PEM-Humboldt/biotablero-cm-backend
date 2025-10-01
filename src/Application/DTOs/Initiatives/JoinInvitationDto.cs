namespace IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Join Invitation dto.
/// </summary>
public class JoinInvitationDto : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Initiative identifier.
    /// </summary>
    public int InitiativeId { get; set; }

    /// <summary>
    /// Creator user identifier.
    /// </summary>
    public string Creator { get; set; }

    /// <summary>
    /// Join Invitation message.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Join Invitation creation date.
    /// </summary>
    public DateTime? CreationDate { get; set; }

    /// <summary>
    /// Join Invitation Guests.
    /// </summary>
    public IEnumerable<JoinInvitationGuestDto> Guests { get; init; }
}
