namespace IAVH.BioTablero.CM.Core.Domain.Utils.Enums;

/// <summary>
/// Initiatives enumerations.
/// </summary>
public static class InitiativesEnums
{
    /// <summary>
    /// Initiative user level.
    /// </summary>
    public enum InitiativeUserLevel
    {
        /// <summary>
        /// User leader level.
        /// </summary>
        Leader = 1,

        /// <summary>
        /// User collaborator level.
        /// </summary>
        Collaborator,

        /// <summary>
        /// User reader level.
        /// </summary>
        Reader,
    }

    /// <summary>
    /// Initiative image type.
    /// </summary>
    public enum InitiativeImageType
    {
        /// <summary>
        /// Initiative image file type.
        /// </summary>
        Image,

        /// <summary>
        /// Initiative banner file type.
        /// </summary>
        Banner,
    }

    /// <summary>
    /// Initiative join request status.
    /// </summary>
    public enum JoinRequestStatus
    {
        /// <summary>
        /// Under review status.
        /// </summary>
        UnderReview = 1,

        /// <summary>
        /// Approved status.
        /// </summary>
        Approved,

        /// <summary>
        /// Rejected status.
        /// </summary>
        Rejected,

        /// <summary>
        /// Cancelled status.
        /// </summary>
        Cancelled,
    }
}
