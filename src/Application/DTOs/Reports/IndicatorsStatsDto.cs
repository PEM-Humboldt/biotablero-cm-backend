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
    /// Indicators by scale data.
    /// </summary>
    public List<KeyValuePair<string, int>> IndicatorsByScale { get; set; } = [];
}
