namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Notification;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.DTOs.Notifications;

/// <summary>
/// Notification OData response example.
/// </summary>
public class NotificationOdataResponseExample : BaseOdataResponseExample<NotificationDto>
{
    /// <inheritdoc/>
    protected override NotificationDto CreateExampleDto() => new()
    {
        Id = 0,
        Receiver = "initiative-user@example.com",
        Subject = "Notification example",
        Body = "Notification example body",
        CreationDate = DateTime.Now,
        ReadingDate = DateTime.Now,
        Readed = true,
        Properties = new()
        {
            TemplateName = "TemplateExample",
            Data = new Dictionary<string, object> { { "key", "value" } },
        },
    };
}
