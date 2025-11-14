namespace IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;

using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Video Helper service interface.
/// </summary>
public interface IVideoHelperService
{
    /// <summary>
    /// Check if video exists.
    /// </summary>
    /// <param name="videoUrl">Video URL.</param>
    /// <param name="ct">Cancellation token (optional).</param>
    /// <returns>True if video exists. False otherwise.</returns>
    Task<bool> VideoExistsAsync(string videoUrl, CancellationToken ct = default);
}
