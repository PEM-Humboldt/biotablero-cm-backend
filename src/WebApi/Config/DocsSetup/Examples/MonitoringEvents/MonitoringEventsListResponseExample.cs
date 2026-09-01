namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.MonitoringEvents;

using System;
using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using IAVH.BioTablero.CM.WebApi.Interfaces;

/// <summary>
/// Monitoring Events list response example.
/// </summary>
public class MonitoringEventsListResponseExample : IOpenApiExampleProvider<List<MonitoringEventsDto>>
{
    /// <inheritdoc/>
    public List<MonitoringEventsDto> GetExamples() =>
    [
        new()
        {
            Id = 0,
            InitiativeId = 0,
            Date = new DateTimeOffset(2026, 1, 1, 0, 0, 0, default),
            Value = 1,
        },
    ];
}
