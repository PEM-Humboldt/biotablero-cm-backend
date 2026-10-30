namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.Indicators;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

/// <summary>
/// Topic repository interface.
/// </summary>
public interface ITopicRepository : IRepository<Topic, int>
{
    /// <summary>
    /// Include OData custom entities.
    /// </summary>
    /// <param name="query">Linq Query.</param>
    /// <returns>Modified Linq query.</returns>
    IQueryable<Topic> IncludeOdataEntities(IQueryable<Topic> query);

    /// <summary>
    /// Get elements by initiative.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities by selected initiative.</returns>
    Task<IEnumerable<Topic>> GetByInitiativeAsync(int initiativeId, CancellationToken ct = default);

    /// <summary>
    /// Get elements by initiative and topic names.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="names">Names list.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Selected entities.</returns>
    Task<IEnumerable<Topic>> GetByInitiativeAndNamesAsync(int initiativeId, string[] names, CancellationToken ct = default);

    /// <summary>
    /// Returns the total number of records.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<int> CountAsync(int initiativeId, CancellationToken ct = default);

    /// <summary>
    /// Check if element is duplicated.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="name">Entity name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    Task<bool> IsDuplicatedAsync(int id, int initiativeId, string name, CancellationToken ct = default);

    /// <summary>
    /// Get topic versions.
    /// </summary>
    /// <param name="id">Topic identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<int[]> GetVersionsAsync(int id, CancellationToken ct = default);
}
