namespace IAVH.BioTablero.CM.Application.Interfaces.General;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Utils;

/// <summary>
/// Enable/Disable data interface (for services).
/// </summary>
/// <typeparam name="TEntityId">Entity identifier type.</typeparam>
public interface IDisable<TEntityId>
    where TEntityId : notnull
{
    /// <summary>
    /// Enable element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> EnableAsync(TEntityId id, CancellationToken ct = default);

    /// <summary>
    /// Disable element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> DisableAsync(TEntityId id, CancellationToken ct = default);
}
