namespace IAVH.BioTablero.CM.Core.Domain.Utils.Email;

/// <summary>
/// Pending requests reminder email data.
/// </summary>
public class PendingRequestsReminderEmailData : DefaultEmailData
{
    /// <summary>
    /// Pending requests count.
    /// </summary>
    public int PendingRequestsCount { get; set; }
}
