namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Notification;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.DTOs.Notifications;

using IAVH.BioTablero.CM.WebApi.Interfaces;

/// <summary>
/// Notification response example.
/// </summary>
public class NotificationResponseExample : IOpenApiExampleProvider<NotificationDto>
{
    /// <inheritdoc/>
    public NotificationDto GetExamples() => new()
    {
        Id = 0,
        Receiver = "initiative-user@example.com",
        Subject = "Notification example",
        Body = "Notification example body",
        CreationDate = DateTimeOffset.UtcNow,
        ReadingDate = DateTimeOffset.UtcNow,
        IsRead = true,
        Properties = new()
        {
            TemplateName = "TemplateExample",
            Data = new Dictionary<string, object?> { { "key", "value" } },
        },
    };
}
