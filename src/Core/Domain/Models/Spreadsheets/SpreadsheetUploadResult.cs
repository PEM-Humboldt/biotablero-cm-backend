namespace IAVH.BioTablero.CM.Core.Domain.Models.Spreadsheets;

using System.Collections.Generic;

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
    /// Process result (saved entities in database).
    /// </summary>
    public object Result { get; set; }

    /// <summary>
    /// Result warnings.
    /// </summary>
    public Dictionary<string, object> Warnings { get; set; } = [];
}
