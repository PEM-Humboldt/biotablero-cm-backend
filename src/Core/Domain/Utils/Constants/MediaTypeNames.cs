namespace IAVH.BioTablero.CM.Core.Domain.Utils.Constants;

/// <summary>
/// Media Type names.
/// </summary>
public static class MediaTypeNames
{
    /// <summary>
    /// Image data types.
    /// </summary>
    public static class Image
    {
        /// <summary>
        /// Joint Photographic Experts Group type.
        /// </summary>
        public const string Jpeg = "image/jpeg";

        /// <summary>
        /// Portable Network Graphics image type.
        /// </summary>
        public const string Png = "image/png";

        /// <summary>
        /// Webp Image content type.
        /// </summary>
        public const string ImageWebp = "image/webp";
    }

    /// <summary>
    /// Application data types.
    /// </summary>
    public static class Application
    {
        /// <summary>
        /// Portable Document File type.
        /// </summary>
        public const string Pdf = "application/pdf";

        /// <summary>
        /// Excel Spreadsheet type.
        /// </summary>
        public const string Xls = "application/vnd.ms-excel";

        /// <summary>
        /// Excel Open XML Spreadsheet type.
        /// </summary>
        public const string Xlsx = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        /// <summary>
        /// PowerPoint Presentation type.
        /// </summary>
        public const string Ppt = "application/vnd.ms-powerpoint";

        /// <summary>
        /// PowerPoint Open XML Presentation type.
        /// </summary>
        public const string Pptx = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
    }
}
