namespace IAVH.BioTablero.CM.Core.Domain.Utils.Email;

/// <summary>
/// Role assignment email data.
/// </summary>
public class RoleAssignmentEmailData : DefaultEmailData
{
    /// <summary>
    /// Initiative name.
    /// </summary>
    public string InitiativeName { get; set; }

    /// <summary>
    /// Level name.
    /// </summary>
    public string LevelName { get; set; }

    /// <summary>
    /// Reviewer user name.
    /// </summary>
    public string ReviewerUserName { get; set; }
}
