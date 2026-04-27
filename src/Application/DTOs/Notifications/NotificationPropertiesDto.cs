namespace IAVH.BioTablero.CM.Application.DTOs.Notifications;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Notification Properties dto.
/// </summary>
public class NotificationPropertiesDto : IDto
{
    /// <summary>
    /// HTML Template Name.
    /// </summary>
    public string TemplateName { get; set; }

    /// <summary>
    /// Message Metadata.
    /// </summary>
    public Dictionary<string, object> Data { get; set; }
}
