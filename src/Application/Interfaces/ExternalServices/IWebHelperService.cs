namespace IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;

using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Web Helper service interface.
/// </summary>
public interface IWebHelperService
{
    /// <summary>
    /// Check if link exists.
    /// </summary>
    /// <param name="url">Link URL.</param>
    /// <param name="ct">Cancellation token (optional).</param>
    /// <returns>True if link exists. False otherwise.</returns>
    Task<bool> LinkExistsAsync(string url, CancellationToken ct = default);
}
