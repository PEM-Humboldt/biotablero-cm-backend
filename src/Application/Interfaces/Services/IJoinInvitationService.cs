namespace IAVH.BioTablero.CM.Application.Interfaces.Services;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using Microsoft.AspNetCore.OData.Query;

/// <summary>
/// Join invitation service interface.
/// </summary>
public interface IJoinInvitationService : IRead<JoinInvitation, int>, IAdd<JoinInvitationDto>
{
    /// <summary>
    /// Get elements list (OData).
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="queryOptions">OData query options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> GetListAsync(int initiativeId, string userName, ODataQueryOptions<JoinInvitation> queryOptions, CancellationToken ct = default);
}
