namespace IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Join Invitation dto.
/// </summary>
[method: SetsRequiredMembers]
public class JoinInvitationDto() : IDto
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
    public required string Creator { get; set; } = string.Empty;

    /// <summary>
    /// Creator user full name.
    /// </summary>
    public string? CreatorFullName { get; set; }

    /// <summary>
    /// Join Invitation message.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Join Invitation HTML message.
    /// </summary>
    public required string HtmlMessage { get; set; } = string.Empty;

    /// <summary>
    /// Initiative name.
    /// </summary>
    public string? InitiativeName { get; set; }

    /// <summary>
    /// Join Invitation creation date.
    /// </summary>
    public DateTimeOffset? CreationDate { get; set; }

    /// <summary>
    /// Join Invitation Guests.
    /// </summary>
    public IEnumerable<JoinInvitationGuestDto>? Guests { get; init; }
}
