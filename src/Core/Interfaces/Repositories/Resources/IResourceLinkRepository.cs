namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.Resources;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;

/// <summary>
/// Resource Link repository interface.
/// </summary>
public interface IResourceLinkRepository : IRepository<ResourceLink, int>
{
    /// <summary>
    /// Get elements by resource.
    /// </summary>
    /// <param name="resourceId">Resource identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities by selected resource.</returns>
    Task<IEnumerable<ResourceLink>> GetByResourceAsync(int resourceId, CancellationToken ct = default);

    /// <summary>
    /// Check if element is duplicated.
    /// </summary>
    /// <param name="resourceId">Resource identifier.</param>
    /// <param name="url">Entity link.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    Task<bool> IsDuplicatedAsync(int resourceId, Uri url, CancellationToken ct = default);

    /// <summary>
    /// Check if element is duplicated.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="resourceId">Resource identifier.</param>
    /// <param name="url">Entity link.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    Task<bool> IsDuplicatedAsync(int id, int resourceId, Uri url, CancellationToken ct = default);
}
