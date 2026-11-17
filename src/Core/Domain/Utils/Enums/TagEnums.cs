namespace IAVH.BioTablero.CM.Core.Domain.Utils.Enums;

/// <summary>
/// Tag enumerations.
/// </summary>
public static class TagEnums
{
    /// <summary>
    /// Tag category.
    /// </summary>
    public enum TagCategory
    {
        #region Initiatives

        /// <summary>
        /// Political context category.
        /// </summary>
        PoliticalContext = 1,

        /// <summary>
        /// Social context category.
        /// </summary>
        SocialContext = 2,

        #endregion

        #region Resources

        /// <summary>
        /// Biological group category.
        /// </summary>
        BiologicalGroup = 3,

        /// <summary>
        /// Ecosystem category.
        /// </summary>
        Ecosystem = 4,

        #endregion
    }
}
