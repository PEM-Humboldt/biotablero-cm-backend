namespace IAVH.BioTablero.CM.Application.Utils;

using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;
using IAVH.BioTablero.CM.Core.Interfaces.ExternalServices;

/// <summary>
/// General file utils.
/// </summary>
public static class FileUtils
{
    #region General

    /// <summary>
    /// Check if a file is empty.
    /// </summary>
    /// <param name="file">Input file.</param>
    /// <returns>True if is empty. False otherwise.</returns>
    public static bool IsEmpty(this IInputFile file) => file == null || file.Size <= 0;

    /// <summary>
    /// Compute hash Sum for stream (SHA256).
    /// </summary>
    /// <param name="stream">File stream.</param>
    /// <returns>SHA256 Sum.</returns>
    public static string ComputeHash(Stream stream)
    {
        // Ensure the stream is at the beginning if it needs to be read from the start
        if (stream.CanSeek)
        {
            stream.Position = 0;
        }

        using var sha1 = SHA1.Create();

        // Compute the hash of the stream
        byte[] hashBytes = sha1.ComputeHash(stream);

        // Convert the byte array to a hexadecimal string
        var builder = new StringBuilder();

        for (int i = 0; i < hashBytes.Length; i++)
        {
            // "x2" formats the byte as a two-digit hexadecimal number
            builder.Append(hashBytes[i].ToString("x2", CultureInfo.InvariantCulture));
        }

        return builder.ToString();
    }

    #endregion

    #region Territory Story

    /// <summary>
    /// Check if is a valid image.
    /// </summary>
    /// <param name="file">Input file.</param>
    /// <returns>True if is valid. False otherwise.</returns>
    public static bool IsValidImage(this IInputFile file)
    {
        var validMimeTypes = new string[]
        {
            MediaTypeNames.Image.Jpeg,
            MediaTypeNames.Image.Png,
        };
        return validMimeTypes.Contains(file?.ContentType);
    }

    /// <summary>
    /// Check if file has valid size (Territory Story Image).
    /// </summary>
    /// <param name="file">Input file.</param>
    /// <returns>True if has valid size. False otherwise.</returns>
    public static bool HasTerritoryStoryImageValidSize(this IInputFile file) => file?.Size <= FileConstants.TerritoryStoryImageSizeLimit;

    #endregion

    #region Resource File

    /// <summary>
    /// Check if is a valid Resource File.
    /// </summary>
    /// <param name="file">Input file.</param>
    /// <returns>True if is valid. False otherwise.</returns>
    public static bool IsValidResourceFile(this IInputFile file)
    {
        var validMimeTypes = new string[]
        {
            MediaTypeNames.Application.Pdf,
            MediaTypeNames.Application.Xls,
            MediaTypeNames.Application.Xlsx,
            MediaTypeNames.Application.Ppt,
            MediaTypeNames.Application.Pptx,
        };
        return validMimeTypes.Contains(file?.ContentType);
    }

    /// <summary>
    /// Check if file has valid size (Resource File).
    /// </summary>
    /// <param name="file">Input file.</param>
    /// <returns>True if has valid size. False otherwise.</returns>
    public static bool HasResourceFileValidSize(this IInputFile file) => file?.Size <= FileConstants.ResourceFileSizeLimit;

    #endregion
}
