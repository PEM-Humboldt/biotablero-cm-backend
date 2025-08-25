namespace IAVH.BioTablero.CM.Application.Specifications;

using Ardalis.Specification;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using JoinRequestStatusEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.JoinRequestStatus;

/// <summary>
/// Join Request ardalis specifications.
/// </summary>
public class JoinRequestSpec : GeneralSpecification<int, JoinRequest>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public JoinRequestSpec()
        : base()
    {
    }

    /// <summary>
    /// Specification for get element by identifier.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    public JoinRequestSpec(int id)
        : base(id)
    {
    }

    /// <summary>
    /// Specification for get pending requests.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="userName">User name.</param>
    /// <returns>Custom specification.</returns>
    public static JoinRequestSpec PendingRequests(int initiativeId, string userName)
    {
        var spec = new JoinRequestSpec();
        spec.Query
            .Where(e => e.InitiativeId == initiativeId && e.UserName == userName && e.StatusId == (int)JoinRequestStatusEnum.UnderReview);

        return spec;
    }
}
