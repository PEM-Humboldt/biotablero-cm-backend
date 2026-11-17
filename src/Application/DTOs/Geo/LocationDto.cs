namespace IAVH.BioTablero.CM.Application.DTOs.Geo;

using System.Diagnostics.CodeAnalysis;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Location DTO.
/// </summary>
[method: SetsRequiredMembers]
public class LocationDto() : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Location name.
    /// </summary>
    public required string Name { get; set; } = string.Empty;

    /// <summary>
    /// Location code.
    /// </summary>
    public required string Code { get; set; } = string.Empty;

    /// <summary>
    /// Parent location relationship.
    /// </summary>
    public LocationDto? Parent { get; set; }

    /// <summary>
    /// Location level.
    /// </summary>
    public byte Level { get; set; }
}
