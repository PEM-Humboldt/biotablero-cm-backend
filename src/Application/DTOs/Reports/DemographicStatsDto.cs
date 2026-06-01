namespace IAVH.BioTablero.CM.Application.DTOs.Reports;

using System.Collections.Generic;

/// <summary>
/// Demographic statistics DTO.
/// </summary>
public class DemographicStatsDto
{
    /// <summary>
    /// Users gender data.
    /// </summary>
    public Dictionary<string, int> Gender { get; set; }

    /// <summary>
    /// Users self-recognition data.
    /// </summary>
    public Dictionary<string, int> SelfRecognition { get; set; }

    /// <summary>
    /// Users organization data.
    /// </summary>
    public Dictionary<string, int> Organization { get; set; }
}
