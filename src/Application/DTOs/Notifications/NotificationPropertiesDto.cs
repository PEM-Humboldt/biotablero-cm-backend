namespace IAVH.BioTablero.CM.Application.DTOs.Notifications;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Notification Properties dto.
/// </summary>
[method: SetsRequiredMembers]
public class NotificationPropertiesDto() : IDto
{
    /// <summary>
    /// HTML Template Name.
    /// </summary>
    public required string TemplateName { get; set; } = string.Empty;

    /// <summary>
    /// Message Metadata.
    /// </summary>
    public required Dictionary<string, object> Data { get; set; } = [];
}
