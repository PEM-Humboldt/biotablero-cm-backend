namespace IAVH.BioTablero.CM.Application.Mappings;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Geo;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;

/// <summary>
/// Initiative mappings
/// </summary>
public class LocationMappings : IMapper<Location, LocationDto>
{
    /// <summary>
    /// Map from entity to DTO
    /// </summary>
    /// <param name="entity">Entity data</param>
    /// <returns>DTO data</returns>
    public LocationDto Map(Location entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Code = entity.Code,
            ParentId = entity.ParentId,
        };
    }

    /// <summary>
    /// Map from DTO to entity
    /// </summary>
    /// <param name="dto">DTO data</param>
    /// <returns>Entity data</returns>
    public Location Map(LocationDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            Id = dto.Id,
            Name = dto.Name,
            Code = dto.Code,
            ParentId = dto.ParentId,
        };
    }
}
