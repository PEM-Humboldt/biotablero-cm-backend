namespace IAVH.BioTablero.CM.Application.Interfaces.Services.Users;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Core.Domain.Models.Iam;

using Microsoft.AspNetCore.OData.Query;

/// <summary>
/// User service interface.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Get user profile data.
    /// </summary>
    /// <param name="userName">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> GetProfileDataAsync(string? userName, CancellationToken ct = default);

    /// <summary>
    /// Get elements list (OData).
    /// </summary>
    /// <param name="queryOptions">OData query options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> GetListAsync(ODataQueryOptions<ExternalUser> queryOptions, CancellationToken ct = default);

    /// <summary>
    /// Get elements list by roles (OData).
    /// </summary>
    /// <param name="roles">Roles list.</param>
    /// <param name="include">Include roles flag.</param>
    /// <param name="query">OData query.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> GetListByRolesAsync(string[] roles, bool include, string query, CancellationToken ct = default);
}
