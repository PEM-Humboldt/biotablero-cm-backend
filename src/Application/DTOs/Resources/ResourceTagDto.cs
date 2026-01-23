namespace IAVH.BioTablero.CM.Application.DTOs.Resources;

using IAVH.BioTablero.CM.Application.DTOs.Tags;
using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Resource Tag dto.
/// </summary>
public class ResourceTagDto : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int ResourceTagId { get; set; }

    /// <summary>
    /// Entity Tag.
    /// </summary>
    public TagDto Tag { get; set; }
}
