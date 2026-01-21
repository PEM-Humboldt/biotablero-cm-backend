namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.Resources;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;
using IAVH.BioTablero.CM.Core.Interfaces.ExternalServices;

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

    /// <summary>
    /// Adds an entity in the database and upload an file in the storage service.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="inputFile">Input file data.</param>
    /// <param name="userName">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<ResourceFile> AddAsync(ResourceFile entity, IInputFile inputFile, string userName, CancellationToken ct = default);

    /// <summary>
    /// Update an entity in the database and upload a file in the storage service.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="inputFile">Input file data.</param>
    /// <param name="userName">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<ResourceFile> UpdateAsync(ResourceFile entity, IInputFile inputFile, string userName, CancellationToken ct = default);
}
