namespace IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Map Legend Item entity.
/// </summary>
public class MapLegendItem : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Map Legend identifier.
    /// </summary>
    public int MapLegendId { get; set; }

    /// <summary>
    /// Map Legend Item color code.
    /// </summary>
    public string ColorCode { get; set; }

    /// <summary>
    /// Map Legend Item value.
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Map Legend relationship.
    /// </summary>
    public MapLegend MapLegend { get; set; }
}
