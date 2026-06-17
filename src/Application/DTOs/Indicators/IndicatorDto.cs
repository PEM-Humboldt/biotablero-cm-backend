namespace IAVH.BioTablero.CM.Application.DTOs.Indicators;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Indicator dto.
/// </summary>
public class IndicatorDto : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Initiative identifier.
    /// </summary>
    public int InitiativeId { get; set; }

    /// <summary>
    /// Indicator Type.
    /// </summary>
    public IndicatorTypeDto Type { get; set; }

    /// <summary>
    /// Indicator Locations relationship.
    /// </summary>
    public IEnumerable<IndicatorLocationDto> Locations { get; init; }

    /// <summary>
    /// Indicator versions list.
    /// </summary>
    public List<IndicatorVersionDto> Versions { get; set; }

    /// <summary>
    /// Tags relationship.
    /// </summary>
    public IEnumerable<IndicatorTagDto> Tags { get; init; }
}
