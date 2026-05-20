namespace IAVH.BioTablero.CM.Core.Domain.Models.User;

/// <summary>
/// User profile data.
/// </summary>
public class UserProfile
{
    /// <summary>
    /// User name.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Total initiatives.
    /// </summary>
    public int TotalInitiatives { get; set; }

    /// <summary>
    /// Total territory stories.
    /// </summary>
    public int TotalTerritoryStories { get; set; }

    /// <summary>
    /// Total resources.
    /// </summary>
    public int TotalResources { get; set; }
}
