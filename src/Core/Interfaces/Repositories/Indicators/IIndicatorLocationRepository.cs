namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.Indicators;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;
using IAVH.BioTablero.CM.Core.Domain.Models.Spreadsheets;

/// <summary>
/// Indicator Location repository interface.
/// </summary>
public interface IIndicatorLocationRepository : IRepository<IndicatorLocation, int>
{
    /// <summary>
    /// Get elements by department, municipality and locality names.
    /// </summary>
    /// <param name="locations">Locations data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Filtered indicator locations by names.</returns>
    Task<List<IndicatorLocation>> GetByNamesAsync(LocationDataHelper[] locations, CancellationToken ct);
}
