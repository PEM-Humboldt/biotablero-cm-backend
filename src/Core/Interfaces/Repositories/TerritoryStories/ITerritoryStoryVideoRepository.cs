namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.TerritoryStories;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;

/// <summary>
/// Territory Story Video repository interface.
/// </summary>
public interface ITerritoryStoryVideoRepository : IRepository<TerritoryStoryVideo, int>
{
    /// <summary>
    /// Get elements by territory story.
    /// </summary>
    /// <param name="territoryStoryId">Territory Story identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities by selected territory story.</returns>
    Task<IEnumerable<TerritoryStoryVideo>> GetByTerritoryStoryAsync(int territoryStoryId, CancellationToken ct = default);

    /// <summary>
    /// Check if element is duplicated.
    /// </summary>
    /// <param name="fileUrl">File URL.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    Task<bool> IsDuplicatedAsync(Uri fileUrl, CancellationToken ct = default);

    /// <summary>
    /// Check if element is duplicated.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="fileUrl">File URL.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    Task<bool> IsDuplicatedAsync(int id, Uri fileUrl, CancellationToken ct = default);
}
