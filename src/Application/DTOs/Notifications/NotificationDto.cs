namespace IAVH.BioTablero.CM.Application.DTOs.Notifications;

using System;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Notification dto.
/// </summary>
public class NotificationDto : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Notification receiver.
    /// </summary>
    public string Receiver { get; set; }

    /// <summary>
    /// Notification subject.
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// Notification body.
    /// </summary>
    public string Body { get; set; }

    /// <summary>
    /// Entity creation date.
    /// </summary>
    public DateTimeOffset CreationDate { get; set; }

    /// <summary>
    /// Entity reading date.
    /// </summary>
    public DateTimeOffset? ReadingDate { get; set; }

    /// <summary>
    /// Is Read flag.
    /// </summary>
    public bool IsRead { get; set; }

    /// <summary>
    /// Entity properties.
    /// </summary>
    public NotificationPropertiesDto Properties { get; set; }
}
