namespace IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.Interfaces.Entities;

/// <summary>
/// Indicator Version Map entity.
/// </summary>
public class IndicatorVersionMap : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Indicator Version identifier.
    /// </summary>
    public int IndicatorVersionId { get; set; }

    /// <summary>
    /// Map title.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Map description.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Map Image URL.
    /// </summary>
    public string ImageUrl { get; set; }

    /// <summary>
    /// Indicator Version relationship.
    /// </summary>
    public IndicatorVersion IndicatorVersion { get; set; }

    /// <summary>
    /// Map Legend relationship.
    /// </summary>
    public ICollection<MapLegend> Legends { get; init; }
}
