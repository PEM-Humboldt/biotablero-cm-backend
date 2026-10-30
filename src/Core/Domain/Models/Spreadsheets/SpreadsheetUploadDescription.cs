namespace IAVH.BioTablero.CM.Core.Domain.Models.Spreadsheets;

/// <summary>
/// Spreadsheet Upload process description.
/// </summary>
public class SpreadsheetUploadDescription
{
    /// <summary>
    /// New observations.
    /// </summary>
    public string[] NewTopics { get; set; } = [];

    /// <summary>
    /// Edited observations.
    /// </summary>
    public string[] EditedTopics { get; set; } = [];
}
