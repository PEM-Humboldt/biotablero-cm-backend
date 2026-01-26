namespace IAVH.BioTablero.CM.Application.Interfaces.Services.Resources;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.DTOs.Resources;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;
using IAVH.BioTablero.CM.Core.Interfaces.ExternalServices;

/// <summary>
/// Resource File service interface.
/// </summary>
public interface IResourceFileService : IRead<ResourceFile, int>
{
    /// <summary>
    /// Get elements by resource.
    /// </summary>
    /// <param name="resourceId">Resource identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities by selected resource.</returns>
    Task<CustomWebResponse> GetByResourceAsync(int resourceId, CancellationToken ct = default);

    /// <summary>
    /// Add element.
    /// </summary>
    /// <param name="userName">User name.</param>
    /// <param name="entityData">Entity data.</param>
    /// <param name="formFile">Image data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> AddAsync(string userName, ResourceFileDto entityData, IInputFile formFile, CancellationToken ct = default);

    /// <summary>
    /// Update element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="entityData">Entity data.</param>
    /// <param name="formFile">Image data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> UpdateAsync(int id, string userName, ResourceFileDto entityData, IInputFile formFile, CancellationToken ct = default);

    /// <summary>
    /// Delete element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> DeleteAsync(int id, string userName, CancellationToken ct = default);
}
