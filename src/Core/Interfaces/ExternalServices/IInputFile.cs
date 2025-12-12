namespace IAVH.BioTablero.CM.Core.Interfaces.ExternalServices;

using System.IO;

/// <summary>
/// Input file data interface.
/// </summary>
public interface IInputFile
{
    /// <summary>
    /// File name.
    /// </summary>
    string FileName { get; }

    /// <summary>
    /// File extension.
    /// </summary>
    string Extension { get; }

    /// <summary>
    /// File Content Type.
    /// </summary>
    string ContentType { get; }

    /// <summary>
    /// File size.
    /// </summary>
    long Size { get; }

    /// <summary>
    /// Open File Stream.
    /// </summary>
    /// <returns>File stream.</returns>
    Stream OpenStream();
}
