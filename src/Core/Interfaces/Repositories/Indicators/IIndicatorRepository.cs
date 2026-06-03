namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.Indicators;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

/// <summary>
/// Indicator repository interface.
/// </summary>
public interface IIndicatorRepository : IRepository<Indicator, int>
{
    /// <summary>
    /// Returns the total number of records.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<int> CountAsync(int initiativeId, CancellationToken ct = default);
}
