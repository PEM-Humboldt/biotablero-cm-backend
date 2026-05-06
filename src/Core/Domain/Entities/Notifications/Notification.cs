namespace IAVH.BioTablero.CM.Core.Domain.Entities.Notifications;

using System;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Notification entity.
/// </summary>
public class Notification : BaseEntity<int>, IAggregateRoot
{
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
    /// Is read flag.
    /// </summary>
    public bool IsRead { get; set; }

    /// <summary>
    /// Entity properties.
    /// </summary>
    public string Properties { get; set; }
}
