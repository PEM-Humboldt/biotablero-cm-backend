namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.MonitoringEvents;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using IAVH.BioTablero.CM.WebApi.Interfaces;

/// <summary>
/// Monitoring Events edit request example.
/// </summary>
public class MonitoringEventsEditRequestExample : IOpenApiExampleProvider<MonitoringEventsDto>
{
    /// <inheritdoc/>
    public MonitoringEventsDto GetExamples() => new()
    {
        Date = new DateTimeOffset(2026, 1, 1, 0, 0, 0, default),
        Value = 1,
    };
}
