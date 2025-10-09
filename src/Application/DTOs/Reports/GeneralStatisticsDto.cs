namespace IAVH.BioTablero.CM.Application.DTOs.Reports;

/// <summary>
/// General statistics DTO for community monitoring.
/// </summary>
public class GeneralStatisticsDto
{
    /// <summary>
    /// Total number of active initiatives in the platform.
    /// </summary>
    public int TotalActiveInitiatives { get; set; }

    /// <summary>
    /// Number of people involved in active initiatives.
    /// </summary>
    public int TotalPeopleInvolved { get; set; }

    /// <summary>
    /// Total area of active initiatives in hectares.
    /// </summary>
    public double TotalAreaInHectares { get; set; }
}
