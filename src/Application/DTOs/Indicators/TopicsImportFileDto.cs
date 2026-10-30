namespace IAVH.BioTablero.CM.Application.DTOs.Indicators;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Topics Import File dto.
/// </summary>
public class TopicsImportFileDto : IDto
{
    /// <summary>
    /// Initiative identifier.
    /// </summary>
    public required int InitiativeId { get; set; }

    /// <summary>
    /// Do not modify database flag.
    /// </summary>
    public bool DoNotModifyDatabase { get; set; } = true;
}
