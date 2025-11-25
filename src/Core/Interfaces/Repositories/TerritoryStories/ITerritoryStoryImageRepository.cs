namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.TerritoryStories;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;
using IAVH.BioTablero.CM.Core.Interfaces.ExternalServices;

/// <summary>
/// Territory Story Image repository interface.
/// </summary>
public interface ITerritoryStoryImageRepository : IRepository<TerritoryStoryImage, int>
{
    /// <summary>
    /// Check authorized entity reading.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if the reading is authorized. False otherwise.</returns>
    Task<bool> AuthorizedEntityReadAsync(int id, string userName, CancellationToken ct = default);

    /// <summary>
    /// Check authorized entity modification.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if the modification is authorized. False otherwise.</returns>
    Task<bool> AuthorizedEntityModifyAsync(int id, string userName, CancellationToken ct = default);

    /// <summary>
    /// Get elements by territory story.
    /// </summary>
    /// <param name="territoryStoryId">Territory Story identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities by selected territory story.</returns>
    Task<IEnumerable<TerritoryStoryImage>> GetByTerritoryStoryAsync(int territoryStoryId, CancellationToken ct = default);

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

    /// <summary>
    /// Mark as featured content.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Updated territory story data.</returns>
    Task<TerritoryStoryImage> MarkAsFeaturedContentAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Adds an entity in the database and upload the image in the storage service.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="formFile">Image data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<TerritoryStoryImage> AddAsync(TerritoryStoryImage entity, IInputFile formFile, CancellationToken ct = default);

    /// <summary>
    /// Update an entity in the database and upload the image in the storage service.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="formFile">Image data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<TerritoryStoryImage> UpdateAsync(TerritoryStoryImage entity, IInputFile formFile, CancellationToken ct = default);
}
