namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.TerritoryStories;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;

/// <summary>
/// Territory Story repository interface.
/// </summary>
public interface ITerritoryStoryRepository : IRepository<TerritoryStory, int>
{
    /// <summary>
    /// Include OData custom entities.
    /// </summary>
    /// <param name="query">Linq Query.</param>
    /// <returns>Modified Linq query.</returns>
    IQueryable<TerritoryStory> IncludeOdataEntities(IQueryable<TerritoryStory> query);

    /// <summary>
    /// Check authorized user action.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities by selected territory story.</returns>
    Task<bool> AuthorizedUserAction(int? id, string userName, CancellationToken ct = default);

    /// <summary>
    /// Get elements by initiative.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities by selected initiative.</returns>
    Task<IEnumerable<TerritoryStory>> GetByInitiativeAsync(int initiativeId, CancellationToken ct = default);

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
    Task<TerritoryStory> MarkAsFeaturedContent(int id, CancellationToken ct = default);
}
