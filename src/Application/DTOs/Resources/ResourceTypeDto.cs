namespace IAVH.BioTablero.CM.Application.DTOs.Resources;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Resource Type dto.
/// </summary>
public class ResourceTypeDto : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Entity name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Entity description.
    /// </summary>
    public string Description { get; set; }
}
