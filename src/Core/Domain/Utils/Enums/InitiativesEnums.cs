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
        /// User member level.
        /// </summary>
        Member,

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
    /// Initiative tag category.
    /// </summary>
    public enum InitiativeTagCategory
    {
        /// <summary>
        /// Political context category.
        /// </summary>
        PoliticalContext = 1,

        /// <summary>
        /// Social context category.
        /// </summary>
        SocialContext,
    }

    /// <summary>
    /// Initiative join request status.
    /// </summary>
    public enum InitiativeJoinRequestStatus
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
    }
}
