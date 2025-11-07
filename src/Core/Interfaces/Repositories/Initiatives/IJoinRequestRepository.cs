namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Join Request repository interface.
/// </summary>
public interface IJoinRequestRepository : IRepository<JoinRequest, int>
{
    /// <summary>
    /// Add initiative filter.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="query">Linq Query.</param>
    /// <returns>Modified Linq query.</returns>
    IQueryable<JoinRequest> AddInitiativeFilter(int initiativeId, IQueryable<JoinRequest> query);

    /// <summary>
    /// Get pending requests.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    Task<bool> AnyPendingRequests(int initiativeId, string userName, CancellationToken ct = default);

    /// <summary>
    /// Review request.
    /// </summary>
    /// <param name="requestId">Request identifier.</param>
    /// <param name="reviewerUserName">Reviewer user name.</param>
    /// <param name="requestStatusId">Request status identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Updated request data.</returns>
    Task<JoinRequest> ReviewRequestAsync(int requestId, string reviewerUserName, int requestStatusId, CancellationToken ct = default);

    /// <summary>
    /// Get pending old requests.
    /// </summary>
    /// <param name="daysOld">Requests days old.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Updated request data.</returns>
    Task<Dictionary<string, int>> GetPendingOldRequestsAsync(int daysOld, CancellationToken ct = default);
}
