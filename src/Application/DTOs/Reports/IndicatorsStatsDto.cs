namespace IAVH.BioTablero.CM.Application.DTOs.Reports;

using System.Collections.Generic;

/// <summary>
/// Indicators statistics DTO.
/// </summary>
public class IndicatorsStatsDto
{
    /// <summary>
    /// Users gender data.
    /// </summary>
    public List<KeyValuePair<string, int>> IndicatorsByScale { get; set; }
}
