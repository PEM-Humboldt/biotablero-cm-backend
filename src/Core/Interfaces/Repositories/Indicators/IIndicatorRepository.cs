namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.Indicators;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

/// <summary>
/// Indicator repository interface.
/// </summary>
public interface IIndicatorRepository : IRepository<Indicator, int>
{
    /// <summary>
    /// Include OData custom entities.
    /// </summary>
    /// <param name="query">Linq Query.</param>
    /// <returns>Modified Linq query.</returns>
    IQueryable<Indicator> IncludeOdataEntities(IQueryable<Indicator> query);

    /// <summary>
    /// Returns the total number of records.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<int> CountAsync(int initiativeId, CancellationToken ct = default);

    /// <summary>
    /// Get indicator versions.
    /// </summary>
    /// <param name="id">Indicator identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<int[]> GetVersions(int id, CancellationToken ct = default);
}
