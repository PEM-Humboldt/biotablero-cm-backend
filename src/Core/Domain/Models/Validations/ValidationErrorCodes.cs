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

    /// <summary>
    /// Initiatives errors.
    /// </summary>
    public static class Initiatives
    {
        /// <summary>
        /// Entity not found.
        /// </summary>
        public const string NotFound = "INI_001";

        /// <summary>
        /// At least one location is required.
        /// </summary>
        public const string LocationsRequired = "INI_002";

        /// <summary>
        /// At least one contact is required.
        /// </summary>
        public const string ContactsRequired = "INI_003";

        /// <summary>
        /// At least one user is required.
        /// </summary>
        public const string UsersRequired = "INI_004";
    }

    /// <summary>
    /// Initiative Join Invitations errors.
    /// </summary>
    public static class JoinInvitations
    {
        /// <summary>
        /// At least one guest is required.
        /// </summary>
        public const string GuestsRequired = "IJI_001";
    }

    /// <summary>
    /// Initiative Join Requests errors.
    /// </summary>
    public static class JoinRequests
    {
        /// <summary>
        /// Invalid user level.
        /// </summary>
        public const string InvalidUserLevel = "IJR_001";
    }

    /// <summary>
    /// Territory Stories errors.
    /// </summary>
    public static class TerritoryStories
    {
        /// <summary>
        /// Entity not found.
        /// </summary>
        public const string NotFound = "STO_001";

        /// <summary>
        /// Entity disabled.
        /// </summary>
        public const string Disabled = "STO_002";

        /// <summary>
        /// Invalid keywords.
        /// </summary>
        public const string InvalidKeywords = "STO_003";
    }

    /// <summary>
    /// Locations errors.
    /// </summary>
    public static class Locations
    {
        /// <summary>
        /// Entity not found.
        /// </summary>
        public const string NotFound = "LOC_001";
    }

    /// <summary>
    /// Tags errors.
    /// </summary>
    public static class Tags
    {
        /// <summary>
        /// Entity not found.
        /// </summary>
        public const string NotFound = "TAG_001";
    }

    /// <summary>
    /// Resources errors.
    /// </summary>
    public static class Resources
    {
        /// <summary>
        /// Entity not found.
        /// </summary>
        public const string NotFound = "RES_001";
    }

    /// <summary>
    /// Resource Types errors.
    /// </summary>
    public static class ResourceTypes
    {
        /// <summary>
        /// Entity not found.
        /// </summary>
        public const string NotFound = "RTY_001";
    }
}
