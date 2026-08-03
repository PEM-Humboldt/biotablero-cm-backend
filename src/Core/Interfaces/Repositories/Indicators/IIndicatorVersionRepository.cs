namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.Indicators;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

/// <summary>
/// Indicator Version repository interface.
/// </summary>
public interface IIndicatorVersionRepository : IRepository<IndicatorVersion, int>
{
    /// <summary>
    /// Get last version number.
    /// </summary>
    /// <param name="indicatorId">Indicator identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The last version for the specified indicator.</returns>
    Task<int> GetLastVersionAsync(int indicatorId, CancellationToken ct = default);
}
