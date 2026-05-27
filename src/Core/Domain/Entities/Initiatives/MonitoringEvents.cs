namespace IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using System;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Monitoring Events entity.
/// </summary>
public class MonitoringEvents : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Initiative identifier.
    /// </summary>
    public int InitiativeId { get; set; }

    /// <summary>
    /// Monitoring events date.
    /// </summary>
    public DateTimeOffset Date { get; set; }

    /// <summary>
    /// Number of monitoring events.
    /// </summary>
    public int Value { get; set; }

    /// <summary>
    /// Initiative relationship.
    /// </summary>
    public Initiative Initiative { get; set; }
}
