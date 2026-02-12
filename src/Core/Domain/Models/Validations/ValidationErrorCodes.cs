namespace IAVH.BioTablero.CM.Core.Domain.Models.Validations;

/// <summary>
/// Validation error codes.
/// </summary>
public static class ValidationErrorCodes
{
    /// <summary>
    /// Validation errors message.
    /// </summary>
    public const string ValidationErrorsMsg = "Validation errors";

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

        /// <summary>
        /// Database error.
        /// </summary>
        public const string DatabaseError = "SYS_007";

        /// <summary>
        /// OData invalid filter.
        /// </summary>
        public const string OdataInvalidFilter = "SYS_008";

        /// <summary>
        /// OData row limit exceeded.
        /// </summary>
        public const string OdataRowLimitExceeded = "SYS_009";
    }

    /// <summary>
    /// Files errors.
    /// </summary>
    public static class Files
    {
        /// <summary>
        /// Empty element.
        /// </summary>
        public const string Empty = "FIL_001";

        /// <summary>
        /// Invalid format.
        /// </summary>
        public const string InvalidFormat = "FIL_002";

        /// <summary>
        /// File processing error.
        /// </summary>
        public const string ProcessingError = "FIL_003";

        /// <summary>
        /// Storage error.
        /// </summary>
        public const string Storage = "FIL_004";

        /// <summary>
        /// Invalid size.
        /// </summary>
        public const string InvalidSize = "FIL_005";
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

        /// <summary>
        /// Invalid locations data.
        /// </summary>
        public const string InvalidLocationsData = "INI_007";

        /// <summary>
        /// Duplicated locations data.
        /// </summary>
        public const string DuplicatedLocationsData = "INI_008";

        /// <summary>
        /// Leaders per initiative.
        /// </summary>
        public const string LeadersPerInitiative = "INI_009";

        /// <summary>
        /// Invalid JSON object.
        /// </summary>
        public const string InvalidJson = "INI_010";

        /// <summary>
        /// Invalid GeoJSON object.
        /// </summary>
        public const string InvalidGeojson = "INI_011";

        /// <summary>
        /// Invalid image type.
        /// </summary>
        public const string InvalidImageType = "INI_012";
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

        /// <summary>
        /// Leader limit exceeded.
        /// </summary>
        public const string LeaderLimitExceeded = "IUS_003";
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

        /// <summary>
        /// Existing users.
        /// </summary>
        public const string ExistingUsers = "IJI_003";

        /// <summary>
        /// Emails sending error.
        /// </summary>
        public const string EmailsSendingError = "IJI_004";
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
    /// Resources errors.
    /// </summary>
    public static class Resources
    {
        /// <summary>
        /// Entity not found.
        /// </summary>
        public const string NotFound = "RES_001";

        /// <summary>
        /// Element duplicated.
        /// </summary>
        public const string Duplicated = "RES_002";
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
    /// Resource Files errors.
    /// </summary>
    public static class ResourceFiles
    {
        /// <summary>
        /// Items limit exceeded.
        /// </summary>
        public const string ItemsLimitExceeded = "RFI_001";
    }

    /// <summary>
    /// Resource Links errors.
    /// </summary>
    public static class ResourceLinks
    {
        /// <summary>
        /// Items limit exceeded.
        /// </summary>
        public const string ItemsLimitExceeded = "RLI_001";

        /// <summary>
        /// Element duplicated.
        /// </summary>
        public const string Duplicated = "RLI_002";

        /// <summary>
        /// URL not found.
        /// </summary>
        public const string UrlNotFound = "RLI_003";
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

        /// <summary>
        /// Element duplicated.
        /// </summary>
        public const string Duplicated = "TAG_002";

        /// <summary>
        /// Element relationships.
        /// </summary>
        public const string HasRelationships = "TAG_003";
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

        /// <summary>
        /// Element duplicated.
        /// </summary>
        public const string Duplicated = "STO_004";

        /// <summary>
        /// Duplicated videos.
        /// </summary>
        public const string DuplicatedVideos = "STO_005";
    }

    /// <summary>
    /// Territory Story Videos errors.
    /// </summary>
    public static class TerritoryStoryVideos
    {
        /// <summary>
        /// Element duplicated.
        /// </summary>
        public const string Duplicated = "SVI_001";

        /// <summary>
        /// Entity not found.
        /// </summary>
        public const string NotFound = "SVI_002";
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
