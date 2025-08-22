namespace IAVH.BioTablero.CM.Application.Specifications;

using Ardalis.Specification;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using InitiativeJoinRequestStatusEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.InitiativeJoinRequestStatus;

/// <summary>
/// Initiative Join Request ardalis specifications.
/// </summary>
public class InitiativeJoinRequestSpec : GeneralSpecification<int, InitiativeJoinRequest>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public InitiativeJoinRequestSpec()
        : base()
    {
    }

    /// <summary>
    /// Specification for get element by identifier.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    public InitiativeJoinRequestSpec(int id)
        : base(id)
    {
    }

    /// <summary>
    /// Specification for get pending requests.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="userName">User name.</param>
    /// <returns>Custom specification.</returns>
    public static InitiativeJoinRequestSpec PendingRequests(int initiativeId, string userName)
    {
        var spec = new InitiativeJoinRequestSpec();
        spec.Query
            .Where(e => e.InitiativeId == initiativeId && e.UserName == userName && e.StatusId == (int)InitiativeJoinRequestStatusEnum.UnderReview);

        return spec;
    }
}
