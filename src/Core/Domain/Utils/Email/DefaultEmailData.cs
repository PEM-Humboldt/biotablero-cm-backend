namespace IAVH.BioTablero.CM.Core.Domain.Utils.Email;

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

    /// <summary>
    /// Initiative name (for email templates).
    /// </summary>
    public string InitiativeName { get; set; }

    /// <summary>
    /// User name (for email templates).
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Reviewer user name (for email templates).
    /// </summary>
    public string ReviewerUserName { get; set; }

    /// <summary>
    /// Level name (for email templates).
    /// </summary>
    public string LevelName { get; set; }

    /// <summary>
    /// Email message (for email templates).
    /// </summary>
    public string EmailMessage { get; set; }

    /// <summary>
    /// Pending requests count (for email templates).
    /// </summary>
    public int? PendingRequestsCount { get; set; }

    /// <summary>
    /// Join request status (for email templates): "Created", "Approved", "Rejected".
    /// </summary>
    public string JoinRequestStatus { get; set; }
}
