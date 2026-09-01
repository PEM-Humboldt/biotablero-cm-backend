namespace IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using System;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Monitoring Events dto.
/// </summary>
public class MonitoringEventsDto() : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Initiative identifier.
    /// </summary>
    public int? InitiativeId { get; set; }

    /// <summary>
    /// Monitoring events date.
    /// </summary>
    public DateTimeOffset Date { get; set; }

    /// <summary>
    /// Number of monitoring events.
    /// </summary>
    public int Value { get; set; }
}
