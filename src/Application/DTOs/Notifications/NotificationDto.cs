namespace IAVH.BioTablero.CM.Application.DTOs.Notifications;

using System;
using System.Diagnostics.CodeAnalysis;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Notification dto.
/// </summary>
[method: SetsRequiredMembers]
public class NotificationDto() : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Notification receiver.
    /// </summary>
    public required string Receiver { get; set; } = string.Empty;

    /// <summary>
    /// Notification subject.
    /// </summary>
    public required string Subject { get; set; } = string.Empty;

    /// <summary>
    /// Notification body.
    /// </summary>
    public required string Body { get; set; } = string.Empty;

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
    public NotificationPropertiesDto? Properties { get; set; }
}
