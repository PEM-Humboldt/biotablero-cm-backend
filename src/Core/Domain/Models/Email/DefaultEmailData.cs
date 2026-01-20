namespace IAVH.BioTablero.CM.Core.Domain.Models.Email;

/// <summary>
/// Default email data.
/// </summary>
public class DefaultEmailData
{
    /// <summary>
    /// Email address.
    /// </summary>
    public CustomEmailAddress Address { get; set; }

    /// <summary>
    /// Email subject.
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// Email content (HTML format).
    /// </summary>
    public string Content { get; set; }
}
