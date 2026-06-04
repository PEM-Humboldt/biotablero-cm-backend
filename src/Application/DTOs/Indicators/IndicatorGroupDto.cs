namespace IAVH.BioTablero.CM.Application.DTOs.Indicators;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Indicator Group dto.
/// </summary>
public class IndicatorGroupDto : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Category relationship.
    /// </summary>
    public CategoryDto Category { get; set; }

    /// <summary>
    /// Indicator Value relationship.
    /// </summary>
    public IEnumerable<IndicatorValueDto> Values { get; init; }
}
