namespace IAVH.BioTablero.CM.Application.DTOs.Reports;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.DTOs.Tags;

/// <summary>
/// General statistics DTO for community monitoring.
/// </summary>
public class GeneralStatsDto
{
    /// <summary>
    /// Total number of enabled initiatives.
    /// </summary>
    public int EnabledInitiatives { get; set; }

    /// <summary>
    /// Number of people involved in enabled initiatives.
    /// </summary>
    public int PeopleInvolved { get; set; }

    /// <summary>
    /// Number of agreements involved in enabled initiatives.
    /// </summary>
    public int AgreementsInvolved { get; set; }

    /// <summary>
    /// Total area of enabled initiatives.
    /// </summary>
    public double Area { get; set; }

    /// <summary>
    /// Ecosystems involved.
    /// </summary>
    public List<TagDto> EcosystemsInvolved { get; set; }
}
