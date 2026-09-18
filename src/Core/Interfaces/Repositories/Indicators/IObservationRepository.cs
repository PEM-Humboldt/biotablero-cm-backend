namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.Indicators;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

/// <summary>
/// Observation repository interface.
/// </summary>
public interface IObservationRepository : IRepository<Observation, int>
{
    /// <summary>
    /// Include OData custom entities.
    /// </summary>
    /// <param name="query">Linq Query.</param>
    /// <returns>Modified Linq query.</returns>
    IQueryable<Observation> IncludeOdataEntities(IQueryable<Observation> query);

    /// <summary>
    /// Get elements by initiative.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities by selected initiative.</returns>
    Task<IEnumerable<Observation>> GetByInitiativeAsync(int initiativeId, CancellationToken ct = default);

    /// <summary>
    /// Get elements by names.
    /// </summary>
    /// <param name="names">Names list.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>selected entities.</returns>
    Task<IEnumerable<Observation>> GetByNamesAsync(string[] names, CancellationToken ct = default);

    /// <summary>
    /// Returns the total number of records.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<int> CountAsync(int initiativeId, CancellationToken ct = default);

    /// <summary>
    /// Get observation versions.
    /// </summary>
    /// <param name="id">Observation identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<int[]> GetVersionsAsync(int id, CancellationToken ct = default);
}
