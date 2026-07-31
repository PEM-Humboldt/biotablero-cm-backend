namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.Indicators;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

/// <summary>
/// Indicator Location repository interface.
/// </summary>
public interface IIndicatorLocationRepository : IRepository<IndicatorLocation, int>
{
    /// <summary>
    /// Get elements by indicator identifier.
    /// </summary>
    /// <param name="indicatorId">Indicator identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities by selected indicator.</returns>
    Task<List<IndicatorLocation>> GetByIndicatorAsync(int indicatorId, CancellationToken ct);
}
