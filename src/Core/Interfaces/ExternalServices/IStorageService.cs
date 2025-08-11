namespace IAVH.BioTablero.CM.Core.Interfaces.ExternalServices;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Storage;

/// <summary>
/// File Storage service interface.
/// </summary>
public interface IStorageService
{
    /// <summary>
    /// Upload file.
    /// </summary>
    /// <param name="fileName">File name.</param>
    /// <param name="file">Upload file data.</param>
    /// <param name="ct">Cancellation token (optional).</param>
    /// <returns>True if the process is successful. False otherwise.</returns>
    Task<bool> UploadFile(string fileName, IInputFile file, CancellationToken ct = default);

    /// <summary>
    /// Download file.
    /// </summary>
    /// <param name="fileName">File name.</param>
    /// <param name="ct">Cancellation token (optional).</param>
    /// <returns>Downloaded file data. Null otherwise.</returns>
    Task<FileData> DownloadFile(string fileName, CancellationToken ct = default);
}
