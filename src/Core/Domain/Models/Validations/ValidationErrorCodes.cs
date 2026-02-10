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

        /// <summary>
        /// Element duplicated.
        /// </summary>
        public const string Duplicated = "SYS_006";
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
        /// Locations required.
        /// </summary>
        public const string LocationsRequired = "INI_002";

        /// <summary>
        /// Contacts required.
        /// </summary>
        public const string ContactsRequired = "INI_003";

        /// <summary>
        /// Users required.
        /// </summary>
        public const string UsersRequired = "INI_004";

        /// <summary>
        /// Duplicated.
        /// </summary>
        public const string Duplicated = "INI_005";

        /// <summary>
        /// Invalid users.
        /// </summary>
        public const string InvalidUsers = "INI_006";
    }

    /// <summary>
    /// Initiative Contacts errors.
    /// </summary>
    public static class InitiativeContacts
    {
        /// <summary>
        /// Duplicated.
        /// </summary>
        public const string Duplicated = "ICO_001";
    }

    /// <summary>
    /// Initiative Locations errors.
    /// </summary>
    public static class InitiativeLocations
    {
        /// <summary>
        /// Duplicated.
        /// </summary>
        public const string Duplicated = "ILO_001";

        /// <summary>
        /// Locality only for municipality.
        /// </summary>
        public const string LocalityOnlyForMunicipality = "ILO_002";

        /// <summary>
        /// Locations required.
        /// </summary>
        public const string LocationsRequired = "ILO_003";
    }

    /// <summary>
    /// Initiative Users errors.
    /// </summary>
    public static class InitiativeUsers
    {
        /// <summary>
        /// Duplicated.
        /// </summary>
        public const string Duplicated = "IUS_001";

        /// <summary>
        /// Leaders required.
        /// </summary>
        public const string LeadersRequired = "IUS_002";
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

        /// <summary>
        /// Duplicated emails.
        /// </summary>
        public const string DuplicatedEmails = "IJI_002";
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

        /// <summary>
        /// Pending join requests.
        /// </summary>
        public const string PendingJoinRequests = "IJR_002";

        /// <summary>
        /// Reviewed join request.
        /// </summary>
        public const string ReviewedJoinRequests = "IJR_003";
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

    /// <summary>
    /// Users errors.
    /// </summary>
    public static class Users
    {
        /// <summary>
        /// Invalid user.
        /// </summary>
        public const string Invalid = "USR_001";
    }
}
