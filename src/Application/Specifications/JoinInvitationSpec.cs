namespace IAVH.BioTablero.CM.Application.Specifications;

using Ardalis.Specification;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Join Invitation ardalis specifications.
/// </summary>
public class JoinInvitationSpec : GeneralSpecification<int, JoinInvitation>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public JoinInvitationSpec()
        : base()
    {
    }

    /// <summary>
    /// Specification for get element by identifier.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    public JoinInvitationSpec(int id)
        : base(id)
    {
        Query
            .Where(e => e.Id == id)
            .Include(e => e.Guests);
    }
}
