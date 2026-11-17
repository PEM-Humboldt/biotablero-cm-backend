namespace IAVH.BioTablero.CM.WebApi.Utils.Requests.Indicators;

using Microsoft.AspNetCore.Http;

/// <summary>
/// Indicators import file request.
/// </summary>
public class IndicatorsImportFileRequest
{
    /// <summary>
    /// Initiative identifier.
    /// </summary>
    public required int InitiativeId { get; set; }

    /// <summary>
    /// Do not modify database flag.
    /// </summary>
    public bool DoNotModifyDatabase { get; set; } = true;

    /// <summary>
    /// General file.
    /// </summary>
    public required IFormFile File { get; set; }
}
