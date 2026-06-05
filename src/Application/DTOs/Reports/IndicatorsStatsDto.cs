namespace IAVH.BioTablero.CM.Application.DTOs.Reports;

using System.Collections.Generic;

/// <summary>
/// Indicators statistics DTO.
/// </summary>
public class IndicatorsStatsDto
{
    /// <summary>
    /// Indicators by scale data.
    /// </summary>
    public List<KeyValuePair<string, int>> IndicatorsByScale { get; set; }
}
