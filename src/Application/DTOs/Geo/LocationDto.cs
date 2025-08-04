namespace IAVH.BioTablero.CM.Application.DTOs.Geo;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Location DTO
/// </summary>
public class LocationDto : IDto
{
    /// <summary>
    /// Item identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Location name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Location code
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Parent location identifier
    /// </summary>
    public int? ParentId { get; set; }

    /// <summary>
    /// Child locations relationship
    /// </summary>
    public ICollection<LocationDto> Children { get; } = [];
}
