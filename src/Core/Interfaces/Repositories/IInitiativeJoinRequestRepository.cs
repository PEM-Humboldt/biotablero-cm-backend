namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Custom Initiative Join Request repository interface.
/// </summary>
public interface IInitiativeJoinRequestRepository : IRepository<InitiativeJoinRequest>
{
    /// <summary>
    /// Review request.
    /// </summary>
    /// <param name="requestId">Request identifier.</param>
    /// <param name="reviewerUserName">Reviewer user name.</param>
    /// <param name="requestStatusId">Request status identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Updated request data.</returns>
    public Task<InitiativeJoinRequest> ReviewRequest(int requestId, string reviewerUserName, int requestStatusId, CancellationToken ct = default);
}
