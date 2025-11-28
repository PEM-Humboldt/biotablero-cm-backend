namespace IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;

using System.IO;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Image Utils service interface.
/// </summary>
public interface IImageUtilsService
{
    /// <summary>
    /// Compress image to JPEG format.
    /// </summary>
    /// <param name="input">Stream input.</param>
    /// <param name="quality">Image quality.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Compressed image format.</returns>
    Task<Stream> CompressToJpegAsync(Stream input, int quality = 75, CancellationToken ct = default);
}
