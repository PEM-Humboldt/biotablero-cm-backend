namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.MonitoringEvents;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using IAVH.BioTablero.CM.WebApi.Interfaces;

/// <summary>
/// Monitoring Events add request example.
/// </summary>
public class MonitoringEventsAddRequestExample : IOpenApiExampleProvider<MonitoringEventsDto>
{
    /// <inheritdoc/>
    public MonitoringEventsDto GetExamples() => new()
    {
        InitiativeId = 0,
        Date = new DateTimeOffset(2026, 1, 1, 0, 0, 0, default),
        Value = 1,
    };
}
