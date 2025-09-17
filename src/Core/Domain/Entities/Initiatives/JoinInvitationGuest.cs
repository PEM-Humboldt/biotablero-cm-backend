namespace IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using IAVH.BioTablero.CM.Core.Domain.Entities;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Join Invitation Guest entity.
/// </summary>
public class JoinInvitationGuest : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Join Invitation identifier.
    /// </summary>
    public int JoinInvitationId { get; set; }

    /// <summary>
    /// Guest Email.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// JoinInvitation relationship.
    /// </summary>
    public JoinInvitation JoinInvitation { get; set; }
}
