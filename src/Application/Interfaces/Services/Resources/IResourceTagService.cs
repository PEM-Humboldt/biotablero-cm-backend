namespace IAVH.BioTablero.CM.Application.Interfaces.Services.Resources;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Domain;

/// <summary>
/// Resource Tag service interface.
/// </summary>
public interface IResourceTagService
{
    /// <summary>
    /// Add element.
    /// </summary>
    /// <param name="userName">User name.</param>
    /// <param name="resourceId">Resource identifier.</param>
    /// <param name="tagId">Tag identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> AddAsync(string userName, int resourceId, int tagId, CancellationToken ct = default);

    /// <summary>
    /// Delete element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> DeleteAsync(int id, string userName, CancellationToken ct = default);
}
