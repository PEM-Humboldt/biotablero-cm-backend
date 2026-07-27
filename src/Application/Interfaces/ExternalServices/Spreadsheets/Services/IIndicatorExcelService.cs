namespace IAVH.BioTablero.CM.Application.Interfaces.ExternalServices.Spreadsheets.Services;

using IAVH.BioTablero.CM.Core.Domain.Models.Spreadsheets;
using IAVH.BioTablero.CM.Core.Interfaces.ExternalServices;

/// <summary>
/// Indicator Excel service interface.
/// </summary>
public interface IIndicatorExcelService
{
    /// <summary>
    /// Get indicators data from spreadsheet.
    /// </summary>
    /// <param name="formFile">Spreadsheet data.</param>
    /// <returns>Process result.</returns>
    SpreadsheetReadResult<IndicatorsImportRow> GetFileData(IInputFile formFile);
}
