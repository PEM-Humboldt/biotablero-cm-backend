namespace IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Join Invitation Guest dto.
/// </summary>
public class JoinInvitationGuestDto : IDto
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
    public string Email { get; set; }
}
