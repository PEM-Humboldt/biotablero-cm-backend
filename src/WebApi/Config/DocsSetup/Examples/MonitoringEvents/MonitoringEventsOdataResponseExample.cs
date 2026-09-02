namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.MonitoringEvents;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;

/// <summary>
/// Monitoring Events list response example.
/// </summary>
public class MonitoringEventsOdataResponseExample : BaseOdataResponseExample<MonitoringEventsDto>
{
    /// <inheritdoc/>
    protected override MonitoringEventsDto CreateExampleDto() => new()
    {
        Id = 0,
        InitiativeId = 0,
        Date = new DateTimeOffset(2026, 1, 1, 0, 0, 0, default),
        Value = 1,
    };
}
