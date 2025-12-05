namespace IAVH.BioTablero.CM.Core.Domain.Utils.Constants;

/// <summary>
/// Constants for Files Management.
/// </summary>
public static class FileConstants
{
    #region Images

    /// <summary>
    /// Webp image max dimension.
    /// Value from here: https://github.com/SixLabors/ImageSharp/blob/main/src/ImageSharp/Formats/Webp/WebpConstants.cs.
    /// </summary>
    public const int WebpMaxDimension = 16383;

    #endregion

    #region Territory Story

    /// <summary>
    /// Territory Story image size limit.
    /// </summary>
    public const int TerritoryStoryImageSizeLimit = 5242880; // 5 MB

    #endregion
}
