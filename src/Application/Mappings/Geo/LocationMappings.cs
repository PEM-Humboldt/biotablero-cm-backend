namespace IAVH.BioTablero.CM.Application.Mappings.Geo;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Geo;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;

/// <summary>
/// Initiative mappings.
/// </summary>
public class LocationMappings : IMapper<Location, LocationDto>
{
    /// <inheritdoc/>
    public LocationDto Map(Location entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Code = entity.Code,
            Parent = entity.Parent != null ? Map(entity.Parent) : null,
        };
    }

    /// <inheritdoc/>
    public Location Map(LocationDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            Name = dto.Name,
            Code = dto.Code,
            Parent = dto.Parent != null ? Map(dto.Parent) : null,
        };
    }
}
