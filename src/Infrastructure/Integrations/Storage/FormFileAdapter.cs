namespace IAVH.BioTablero.CM.Infrastructure.Integrations.Storage;

using System.IO;

using IAVH.BioTablero.CM.Core.Interfaces.ExternalServices;

using Microsoft.AspNetCore.Http;

/// <summary>
/// Form File adapter.
/// </summary>
/// <param name="file">Form File data.</param>
public class FormFileAdapter(IFormFile file) : IInputFile
{
    private readonly IFormFile file = file;

    /// <summary>
    /// File name.
    /// </summary>
    public string Name => file.FileName;

    /// <summary>
    /// File Content Type.
    /// </summary>
    public string ContentType => file.ContentType;

    /// <summary>
    /// File size.
    /// </summary>
    public long Size => file.Length;

    /// <summary>
    /// Open File Stream.
    /// </summary>
    /// <returns>File stream.</returns>
    public Stream OpenStream() => file.OpenReadStream();
}
