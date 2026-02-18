namespace IAVH.BioTablero.CM.Application.Interfaces.Services.Resources;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Resources;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;

using Microsoft.AspNetCore.OData.Query;

/// <summary>
/// Resource service interface.
/// </summary>
public interface IResourceService : IRead<Resource, int>
{
    /// <summary>
    /// Get element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> GetItemAsync(int id, string userName, CancellationToken ct = default);

    /// <summary>
    /// Get entities by initiative (OData).
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="queryOptions">OData query options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> GetByInitiativeAsync(int initiativeId, string userName, ODataQueryOptions<Resource> queryOptions, CancellationToken ct = default);

    /// <summary>
    /// Add element.
    /// </summary>
    /// <param name="userName">User name.</param>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> AddAsync(string userName, ResourceDto entityData, CancellationToken ct = default);

    /// <summary>
    /// Update element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> UpdateAsync(int id, string userName, ResourceDto entityData, CancellationToken ct = default);

    /// <summary>
    /// Like action.
    /// </summary>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> LikeActionAsync(ResourceLikeDto entityData, CancellationToken ct = default);

    /// <summary>
    /// Delete element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> DeleteAsync(int id, string userName, CancellationToken ct = default);

    /// <summary>
    /// Send notification for resource update.
    /// </summary>
    /// <param name="resource">Resource data.</param>
    /// <param name="userName">Editor user name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if the process is successful. False otherwise.</returns>
    Task<bool> SendUpdateNotificationAsync(Resource resource, string userName, CancellationToken ct = default);
}
