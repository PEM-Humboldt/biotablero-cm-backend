namespace IAVH.BioTablero.CM.Application.Interfaces.Services.Initiatives;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Domain;

/// <summary>
/// Initiative Tag service interface.
/// </summary>
public interface IInitiativeTagService
{
    /// <summary>
    /// Add element.
    /// </summary>
    /// <param name="userName">User name.</param>
    /// <param name="userIsAdmin">User administrator flag.</param>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="tagId">Tag identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> AddAsync(string userName, bool userIsAdmin, int initiativeId, int tagId, CancellationToken ct = default);

    /// <summary>
    /// Delete element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="userIsAdmin">User administrator flag.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> DeleteAsync(int id, string userName, bool userIsAdmin, CancellationToken ct = default);
}
