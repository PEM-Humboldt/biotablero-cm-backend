namespace IAVH.BioTablero.CM.Core.Domain.Models.Spreadsheets;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Observation Import spreadsheet row.
/// </summary>
[method: SetsRequiredMembers]
public class ObservationImportRow()
{
    /// <summary>
    /// Spreadsheet cell row number.
    /// </summary>
    public int RowNumber { get; set; }

    /// <summary>
    /// Initiative identifier.
    /// </summary>
    public required int InitiativeId { get; set; }

    /// <summary>
    /// Observation Topic identifier.
    /// </summary>
    public required int IndicatorTopicId { get; set; }

    /// <summary>
    /// Indicator Type identifier.
    /// </summary>
    public required int IndicatorTypeId { get; set; }

    /// <summary>
    /// Department name.
    /// </summary>
    public required string DepartmentName { get; set; } = string.Empty;

    /// <summary>
    /// Municipality name.
    /// </summary>
    public required string MunicipalityName { get; set; } = string.Empty;

    /// <summary>
    /// Locality name.
    /// </summary>
    public required string LocalityName { get; set; } = string.Empty;

    /// <summary>
    /// Year.
    /// </summary>
    public required string Year { get; set; } = string.Empty;

    /// <summary>
    /// Month.
    /// </summary>
    public required string Month { get; set; } = string.Empty;

    /// <summary>
    /// Final year.
    /// </summary>
    public string? FinalYear { get; set; }

    /// <summary>
    /// Final month.
    /// </summary>
    public string? FinalMonth { get; set; }

    /// <summary>
    /// Upper group name.
    /// </summary>
    public required string UpperGroupName { get; set; } = string.Empty;

    /// <summary>
    /// Group name.
    /// </summary>
    public string? GroupName { get; set; }

    /// <summary>
    /// Group description.
    /// </summary>
    public string? GroupDescription { get; set; }

    /// <summary>
    /// Observation value.
    /// </summary>
    public float Value { get; set; }

    /// <summary>
    /// Observation Value upper limit.
    /// </summary>
    public float? UpperLimit { get; set; }

    /// <summary>
    /// Observation Value lower limit.
    /// </summary>
    public float? LowerLimit { get; set; }
}
