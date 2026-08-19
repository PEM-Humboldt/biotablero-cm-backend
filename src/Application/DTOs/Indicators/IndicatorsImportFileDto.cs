namespace IAVH.BioTablero.CM.Application.DTOs.Indicators;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Indicators Import File dto.
/// </summary>
public class IndicatorsImportFileDto : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Initiative identifier.
    /// </summary>
    public required int InitiativeId { get; set; }

    /// <summary>
    /// Do not modify database flag.
    /// </summary>
    public bool DoNotModifyDatabase { get; set; } = true;
}
