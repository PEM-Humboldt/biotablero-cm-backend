namespace IAVH.BioTablero.CM.Core.Domain.Models.Storage;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// General file data.
/// </summary>
[method: SetsRequiredMembers]
public class FileData()
{
    /// <summary>
    /// File name.
    /// </summary>
    public required string Name { get; set; } = "Empty";

    /// <summary>
    /// MIME Type.
    /// </summary>
    public required string MimeType { get; set; } = "Empty";

    /// <summary>
    /// File content data.
    /// </summary>
    public byte[]? File { get; set; }
}
