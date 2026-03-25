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
    public DateTime CreationDate { get; set; }

    /// <summary>
    /// Entity reading date.
    /// </summary>
    public DateTime? ReadingDate { get; set; }

    /// <summary>
    /// Readed flag.
    /// </summary>
    public bool Readed { get; set; }

    /// <summary>
    /// Entity properties.
    /// </summary>
    public string Properties { get; set; }
}
