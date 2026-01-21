namespace IAVH.BioTablero.CM.Core.Domain.Models.Email;

/// <summary>
/// Update resource email data.
/// </summary>
public class UpdateResourceEmailData : DefaultEmailData
{
    /// <summary>
    /// Resource name.
    /// </summary>
    public string ResourceName { get; set; }

    /// <summary>
    /// Editor user name.
    /// </summary>
    public string EditorUserName { get; set; }
}
