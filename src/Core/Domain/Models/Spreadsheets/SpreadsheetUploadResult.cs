namespace IAVH.BioTablero.CM.Core.Domain.Models.Spreadsheets;

/// <summary>
/// Spreadsheet Upload Result.
/// </summary>
public class SpreadsheetUploadResult()
{
    /// <summary>
    /// Do not modify database flag.
    /// </summary>
    public bool DoNotModifyDatabase { get; set; }

    /// <summary>
    /// Successful process flag.
    /// </summary>
    public bool SuccessfulProcess { get; set; }

    /// <summary>
    /// Process result (saved entities in database).
    /// </summary>
    public object Result { get; set; }
}
