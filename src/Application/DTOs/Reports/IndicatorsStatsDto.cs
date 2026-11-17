namespace IAVH.BioTablero.CM.Application.DTOs.Reports;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Indicators statistics DTO.
/// </summary>
[method: SetsRequiredMembers]
public class IndicatorsStatsDto()
{
    /// <summary>
    /// Topics by scale data.
    /// </summary>
    public List<KeyValuePair<string, int>> TopicsByScale { get; set; } = [];
}
