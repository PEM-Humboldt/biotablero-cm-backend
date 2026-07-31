namespace IAVH.BioTablero.CM.Core.Domain.Models.Spreadsheets;

/// <summary>
/// Category comparison helper.
/// </summary>
public class CategoryComparisonHelper()
{
    /// <summary>
    /// Category data from spreadsheet.
    /// </summary>
    public GroupDataHelper CategorySpreadsheet { get; set; }

    /// <summary>
    /// Category data from database.
    /// </summary>
    public GroupDataHelper CategoryDb { get; set; }
}
