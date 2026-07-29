namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.Indicators;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

/// <summary>
/// Indicator Category repository interface.
/// </summary>
public interface ICategoryRepository : IRepository<Category, int>
{
    /// <summary>
    /// Get upper groups by category names.
    /// </summary>
    /// <param name="categoryNames">Category names list.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Filtered categories by names.</returns>
    Task<List<Category>> GetUpperGroupsAsync(string[] categoryNames, CancellationToken ct);
}
