namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.Locations;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;

/// <summary>
/// Location repository interface.
/// </summary>
public interface ILocationRepository : IRepository<Location, int>
{
    /// <summary>
    /// Get elements by parent identifier.
    /// </summary>
    /// <param name="parentId">Parent identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of elements by specified parent.</returns>
    public Task<IEnumerable<Location>> GetByParentIdAsync(int? parentId, CancellationToken ct = default);
}
