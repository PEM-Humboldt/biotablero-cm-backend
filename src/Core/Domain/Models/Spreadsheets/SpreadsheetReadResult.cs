namespace IAVH.BioTablero.CM.Core.Domain.Models.Spreadsheets;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Spreadsheet Read Result.
/// </summary>
/// <typeparam name="TRow">Row type.</typeparam>
[method: SetsRequiredMembers]
public class SpreadsheetReadResult<TRow>()
    where TRow : class
{
    /// <summary>
    /// Spreadsheet rows data.
    /// </summary>
    public required List<TRow> Rows { get; set; } = [];

    /// <summary>
    /// Errors found.
    /// </summary>
    public required List<string> Errors { get; set; } = [];
}
