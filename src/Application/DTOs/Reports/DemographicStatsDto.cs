namespace IAVH.BioTablero.CM.Application.DTOs.Reports;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Demographic statistics DTO.
/// </summary>
[method: SetsRequiredMembers]
public class DemographicStatsDto()
{
    /// <summary>
    /// Users gender data.
    /// </summary>
    public required List<KeyValuePair<string, int>> Gender { get; set; } = [];

    /// <summary>
    /// Users self-recognition data.
    /// </summary>
    public required List<KeyValuePair<string, int>> SelfRecognition { get; set; } = [];

    /// <summary>
    /// Users organization data.
    /// </summary>
    public required List<KeyValuePair<string, int>> Organization { get; set; } = [];
}
