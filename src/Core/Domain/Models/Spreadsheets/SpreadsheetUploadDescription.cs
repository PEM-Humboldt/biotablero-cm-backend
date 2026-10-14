namespace IAVH.BioTablero.CM.Core.Domain.Models.Spreadsheets;

/// <summary>
/// Spreadsheet Upload process description.
/// </summary>
public class SpreadsheetUploadDescription
{
    /// <summary>
    /// New observations.
    /// </summary>
    public string[] NewObservations { get; set; } = [];

    /// <summary>
    /// Edited observations.
    /// </summary>
    public string[] EditedObservations { get; set; } = [];
}
