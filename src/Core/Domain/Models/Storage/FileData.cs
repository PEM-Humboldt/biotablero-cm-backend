namespace IAVH.BioTablero.CM.Core.Domain.Models.Storage;

/// <summary>
/// General file data.
/// </summary>
public class FileData
{
    /// <summary>
    /// File name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// MIME Type.
    /// </summary>
    public string MimeType { get; set; }

    /// <summary>
    /// File content data.
    /// </summary>
    public byte[] File { get; set; }
}
