namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Custom Initiative Join Request repository interface.
/// </summary>
public interface IInitiativeJoinRequestRepository : IRepository<InitiativeJoinRequest>
{
    /// <summary>
    /// Add initiative filter.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="query">Linq Query.</param>
    /// <returns>Modified Linq query.</returns>
    public IQueryable<InitiativeJoinRequest> AddInitiativeFilter(int initiativeId, IQueryable<InitiativeJoinRequest> query);

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
