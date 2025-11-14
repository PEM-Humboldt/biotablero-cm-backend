namespace IAVH.BioTablero.CM.Application.Interfaces.Services.TerritoryStory;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;

/// <summary>
/// Territory Story Video service interface.
/// </summary>
public interface ITerritoryStoryVideoService : IRead<TerritoryStoryVideo, int>
{
    /// <summary>
    /// Get elements by territory story.
    /// </summary>
    /// <param name="territoryStoryId">Territory Story identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities by selected territory story.</returns>
    Task<CustomWebResponse> GetByTerritoryStoryAsync(int territoryStoryId, CancellationToken ct = default);

    /// <summary>
    /// Add element.
    /// </summary>
    /// <param name="userName">User name.</param>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> AddAsync(string userName, TerritoryStoryVideoDto entityData, CancellationToken ct = default);

    /// <summary>
    /// Update element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> UpdateAsync(int id, string userName, TerritoryStoryVideoDto entityData, CancellationToken ct = default);

    /// <summary>
    /// Delete element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> DeleteAsync(int id, string userName, CancellationToken ct = default);
}
