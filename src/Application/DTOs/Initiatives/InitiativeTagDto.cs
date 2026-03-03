namespace IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using IAVH.BioTablero.CM.Application.DTOs.Tags;
using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Initiative Tag dto.
/// </summary>
public class InitiativeTagDto : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int InitiativeTagId { get; set; }

    /// <summary>
    /// Entity Tag.
    /// </summary>
    public TagDto Tag { get; set; }
}
