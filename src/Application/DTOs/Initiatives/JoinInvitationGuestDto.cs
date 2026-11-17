namespace IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using System.Diagnostics.CodeAnalysis;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Join Invitation Guest dto.
/// </summary>
[method: SetsRequiredMembers]
public class JoinInvitationGuestDto() : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Join Invitation identifier.
    /// </summary>
    public int? JoinInvitationId { get; set; }

    /// <summary>
    /// Guest Email.
    /// </summary>
    public required string Email { get; set; } = string.Empty;
}
