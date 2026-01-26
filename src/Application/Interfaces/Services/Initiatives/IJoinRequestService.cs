namespace IAVH.BioTablero.CM.Application.Interfaces.Services.Initiatives;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using Microsoft.AspNetCore.OData.Query;

/// <summary>
/// Join request service interface.
/// </summary>
public interface IJoinRequestService : IRead<JoinRequest, int>, IAdd<JoinRequestDto>, IUpdate<JoinRequestDto, int>
{
    /// <summary>
    /// Get elements list (OData).
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="queryOptions">OData query options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> GetListAsync(int initiativeId, string userName, ODataQueryOptions<JoinRequest> queryOptions, CancellationToken ct = default);

    /// <summary>
    /// Send notifications for old pending join requests.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task SendNotificationsOldPendingRequestsAsync(CancellationToken ct = default);
}
