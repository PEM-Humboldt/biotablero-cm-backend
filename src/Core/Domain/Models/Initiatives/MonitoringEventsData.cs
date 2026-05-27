namespace IAVH.BioTablero.CM.Core.Domain.Models.Initiatives;

/// <summary>
/// Monitoring Events data.
/// </summary>
public class MonitoringEventsData
{
    /// <summary>
    /// Monitoring events date or group.
    /// </summary>
    public int GroupNumber { get; set; }

    /// <summary>
    /// Monitoring events date or group.
    /// </summary>
    public string GroupName { get; set; }

    /// <summary>
    /// Number of monitoring events by date.
    /// </summary>
    public int Value { get; set; }
}
