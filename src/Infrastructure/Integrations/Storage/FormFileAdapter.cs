namespace IAVH.BioTablero.CM.Infrastructure.Integrations.Storage;

using System.IO;

using IAVH.BioTablero.CM.Core.Interfaces.ExternalServices;

using Microsoft.AspNetCore.Http;

/// <summary>
/// IFormFile adapter.
/// </summary>
/// <param name="file">Form File data.</param>
public class FormFileAdapter(IFormFile file) : IInputFile
{
    private readonly IFormFile file = file;

    /// <inheritdoc/>
    public string FileName => file?.FileName;

    /// <inheritdoc/>
    public string Extension => Path.GetExtension(FileName);

    /// <inheritdoc/>
    public string ContentType => file?.ContentType;

    /// <inheritdoc/>
    public long Size => file?.Length ?? 0;

    /// <inheritdoc/>
    public Stream OpenStream() => file?.OpenReadStream();
}
