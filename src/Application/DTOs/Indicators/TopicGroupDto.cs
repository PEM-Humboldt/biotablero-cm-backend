namespace IAVH.BioTablero.CM.Application.DTOs.Indicators;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Topic Group dto.
/// </summary>
public class TopicGroupDto : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Category relationship.
    /// </summary>
    public CategoryDto? Category { get; set; }

    /// <summary>
    /// Indicator Value relationship.
    /// </summary>
    public IEnumerable<IndicatorValueDto>? Values { get; init; }
}
