namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.Resources;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;

/// <summary>
/// Resource File repository interface.
/// </summary>
public interface IResourceFileRepository : IRepository<ResourceFile, int>
{
    /// <summary>
    /// Get elements by resource.
    /// </summary>
    /// <param name="resourceId">Resource identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities by selected resource.</returns>
    Task<IEnumerable<ResourceFile>> GetByResourceAsync(int resourceId, CancellationToken ct = default);
}
