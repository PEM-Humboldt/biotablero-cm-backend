namespace IAVH.BioTablero.CM.Application.Utils;

using System.Linq;

using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;
using IAVH.BioTablero.CM.Core.Interfaces.ExternalServices;

/// <summary>
/// General file utils.
/// </summary>
public static class FileUtils
{
    /// <summary>
    /// Check if a file is empty.
    /// </summary>
    /// <param name="file">Input file.</param>
    /// <returns>True if is empty. False otherwise.</returns>
    public static bool IsEmpty(this IInputFile file) => file == null || file.Size <= 0;

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
}
