namespace IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Map Legend entity.
/// </summary>
public class MapLegend : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Indicator Version Map identifier.
    /// </summary>
    public int IndicatorVersionMapId { get; set; }

    /// <summary>
    /// Map Legend title.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Indicator Version Map relationship.
    /// </summary>
    public IndicatorVersionMap IndicatorVersionMap { get; set; }

    /// <summary>
    /// Map Legend Item relationship.
    /// </summary>
    public ICollection<MapLegendItem> Items { get; init; }
}
