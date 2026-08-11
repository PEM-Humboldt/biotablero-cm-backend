namespace IAVH.BioTablero.CM.Core.Domain.Models.Spreadsheets;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Indicators Import spreadsheet row.
/// </summary>
[method: SetsRequiredMembers]
public class IndicatorsImportRow()
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
    /// Indicator Type identifier.
    /// </summary>
    public required int IndicatorTypeId { get; set; }

    /// <summary>
    /// measure Unit identifier.
    /// </summary>
    public required int MeasureUnitId { get; set; }

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
    public required string Year { get; set; }

    /// <summary>
    /// Month.
    /// </summary>
    public required string Month { get; set; }

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
    public required string UpperGroupName { get; set; }

    /// <summary>
    /// Group name.
    /// </summary>
    public string? GroupName { get; set; }

    /// <summary>
    /// Group description.
    /// </summary>
    public string? GroupDescription { get; set; }

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
