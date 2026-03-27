namespace IAVH.BioTablero.CM.Application.Domain;

using IAVH.BioTablero.CM.Application.DTOs.Notifications;

/// <summary>
/// Send Notification data.
/// </summary>
public class SendNotificationData
{
    /// <summary>
    /// Notification data.
    /// </summary>
    public NotificationDto NotificationDto { get; set; }

    /// <summary>
    /// Send email flag.
    /// </summary>
    public bool SendEmail { get; set; } = true;

    /// <summary>
    /// Send to hidden receivers flag.
    /// </summary>
    public bool SendToHiddenReceivers { get; set; } = true;

    /// <summary>
    /// Initiative identifier (optional).
    /// </summary>
    public int? InitiativeId { get; set; }
}
