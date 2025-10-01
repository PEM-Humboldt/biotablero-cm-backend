namespace IAVH.BioTablero.CM.Application.DTOs.Reports;

/// <summary>
/// General statistics DTO for community monitoring.
/// </summary>
public class GeneralStatisticsDto
{
    /// <summary>
    /// Número total de iniciativas activas en la plataforma.
    /// </summary>
    public int TotalActiveInitiatives { get; set; }

    /// <summary>
    /// Número de personas involucradas en iniciativas activas.
    /// </summary>
    public int TotalPeopleInvolved { get; set; }

    /// <summary>
    /// Área total de las iniciativas activas en hectáreas.
    /// </summary>
    public double TotalAreaInHectares { get; set; }
}
