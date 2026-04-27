namespace IAVH.BioTablero.CM.Application.DTOs.Geo;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Location DTO.
/// </summary>
public class LocationDto : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Location name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Location code.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Parent location relationship.
    /// </summary>
    public LocationDto Parent { get; set; }

    /// <summary>
    /// Location level.
    /// </summary>
    public byte Level { get; set; }
}
