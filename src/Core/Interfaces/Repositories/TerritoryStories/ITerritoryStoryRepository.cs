namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.TerritoryStories;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;

/// <summary>
/// Territory Story repository interface.
/// </summary>
public interface ITerritoryStoryRepository : IRepository<TerritoryStory, int>
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
    Task<bool> AuthorizedEntityModifyAsync(int? id, string userName, CancellationToken ct = default);

    /// <summary>
    /// Get elements by initiative.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities by selected initiative.</returns>
    Task<IEnumerable<TerritoryStory>> GetByInitiativeAndUserNameAsync(int initiativeId, string userName, CancellationToken ct = default);

    /// <summary>
    /// Check if element is duplicated.
    /// </summary>
    /// <param name="title">Entity title.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    Task<bool> IsDuplicatedAsync(string title, CancellationToken ct = default);

    /// <summary>
    /// Check if element is duplicated.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="title">Entity title.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    Task<bool> IsDuplicatedAsync(int id, string title, CancellationToken ct = default);

    /// <summary>
    /// Mark as featured content.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Updated territory story data.</returns>
    Task<TerritoryStory> MarkAsFeaturedContentAsync(int id, CancellationToken ct = default);
}
