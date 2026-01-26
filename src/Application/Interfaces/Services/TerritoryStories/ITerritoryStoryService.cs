namespace IAVH.BioTablero.CM.Application.Interfaces.Services.TerritoryStories;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;

using Microsoft.AspNetCore.OData.Query;

/// <summary>
/// Territory Story service interface.
/// </summary>
public interface ITerritoryStoryService : IRead<TerritoryStory, int>, IAdd<TerritoryStoryDto>
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
    Task<CustomWebResponse> GetByInitiativeAsync(int initiativeId, string userName, ODataQueryOptions<TerritoryStory> queryOptions, CancellationToken ct = default);

    /// <summary>
    /// Update element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> UpdateAsync(int id, string userName, TerritoryStoryDto entityData, CancellationToken ct = default);

    /// <summary>
    /// Like action.
    /// </summary>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> LikeActionAsync(TerritoryStoryLikeDto entityData, CancellationToken ct = default);

    /// <summary>
    /// Featured content action.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> FeaturedContentActionAsync(int id, string userName, CancellationToken ct = default);

    /// <summary>
    /// Enable element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> EnableAsync(int id, string userName, CancellationToken ct = default);

    /// <summary>
    /// Disable element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> DisableAsync(int id, string userName, CancellationToken ct = default);
}
