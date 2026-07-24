namespace IAVH.BioTablero.CM.Application.DTOs.Indicators;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Indicators Import spreadsheet row dto.
/// </summary>
public class IndicatorsImportRowDto : IDto
{
    /// <summary>
    /// Indicator identifier.
    /// </summary>
    public required string IndicatorId { get; set; }

    /// <summary>
    /// Indicator name.
    /// </summary>
    public required string IndicatorName { get; set; }

    /// <summary>
    /// Initiative name.
    /// </summary>
    public required string InitiativeName { get; set; }

    /// <summary>
    /// Department name.
    /// </summary>
    public required string DepartmentName { get; set; }

    /// <summary>
    /// Municipality name.
    /// </summary>
    public required string MunicipalityName { get; set; }

    /// <summary>
    /// Locality name.
    /// </summary>
    public required string LocalityName { get; set; }

    /// <summary>
    /// Year.
    /// </summary>
    public required int Year { get; set; }

    /// <summary>
    /// Month.
    /// </summary>
    public required int Month { get; set; }

    /// <summary>
    /// Upper group.
    /// </summary>
    public required string UpperGroup { get; set; }

    /// <summary>
    /// Scientific name.
    /// </summary>
    public string ScientificName { get; set; }

    /// <summary>
    /// Common name.
    /// </summary>
    public string CommonName { get; set; }

    /// <summary>
    /// Indicator value.
    /// </summary>
    public float Value { get; set; }

    /// <summary>
    /// Indicator Value upper limit.
    /// </summary>
    public float? UpperLimit { get; set; }

    /// <summary>
    /// Indicator Value lower limit.
    /// </summary>
    public float? LowerLimit { get; set; }
}
