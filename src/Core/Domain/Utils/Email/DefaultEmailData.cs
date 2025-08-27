namespace IAVH.BioTablero.CM.Core.Domain.Utils.Email;

/// <summary>
/// Default email data.
/// </summary>
public class DefaultEmailData
{
    /// <summary>
    /// Receiver name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Receiver email.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Email subject.
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// Email content (HTML format).
    /// </summary>
    public string Content { get; set; }
}
