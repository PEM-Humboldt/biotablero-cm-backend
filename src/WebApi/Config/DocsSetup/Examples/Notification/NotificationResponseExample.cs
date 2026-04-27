namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Notification;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.DTOs.Notifications;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Notification response example.
/// </summary>
public class NotificationResponseExample : IExamplesProvider<NotificationDto>
{
    /// <inheritdoc/>
    public NotificationDto GetExamples() => new()
    {
        Id = 0,
        Receiver = "initiative-user@example.com",
        Subject = "Notification example",
        Body = "Notification example body",
        CreationDate = DateTime.Now,
        ReadingDate = DateTime.Now,
        IsRead = true,
        Properties = new()
        {
            TemplateName = "TemplateExample",
            Data = new Dictionary<string, object> { { "key", "value" } },
        },
    };
}
