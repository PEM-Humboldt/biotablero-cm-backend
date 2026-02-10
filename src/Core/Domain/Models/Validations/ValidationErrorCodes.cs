namespace IAVH.BioTablero.CM.Core.Domain.Models.Validations;

/// <summary>
/// Validation error codes.
/// </summary>
public static class ValidationErrorCodes
{
    /// <summary>
    /// General errors.
    /// </summary>
    public static class General
    {
        /// <summary>
        /// Empty request entity data error.
        /// </summary>
        public const string EmptyEntityData = "SYS_001";

        /// <summary>
        /// Empty property data error.
        /// </summary>
        public const string EmptyProperty = "SYS_002";

        /// <summary>
        /// Invalid property value error.
        /// </summary>
        public const string InvalidPropertyValue = "SYS_003";

        /// <summary>
        /// Element not found.
        /// </summary>
        public const string ElementNotFound = "SYS_004";

        /// <summary>
        /// Element disabled.
        /// </summary>
        public const string ElementDisabled = "SYS_005";
    }
}
