namespace IAVH.BioTablero.CM.Application.Domain;

using IAVH.BioTablero.CM.Application.DTOs.Notifications;
using IAVH.BioTablero.CM.Core.Domain.Models.Email;

/// <summary>
/// Send Notification data.
/// </summary>
public class SendNotificationData
{
    /// <summary>
    /// Notification data.
    /// </summary>
    public required NotificationDto NotificationDto { get; set; }

    /// <summary>
    /// Notification receivers list.
    /// </summary>
    public required CustomEmailAddress[] Receivers { get; set; }

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
