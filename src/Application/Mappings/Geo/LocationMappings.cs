namespace IAVH.BioTablero.CM.Application.Mappings.Geo;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Geo;
using IAVH.BioTablero.CM.Application.Mappings.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;

/// <summary>
/// Initiative mappings.
/// </summary>
public class LocationMappings : MapperRead<Location, LocationDto>
{
    /// <inheritdoc/>
    public override LocationDto Map(Location entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Code = entity.Code,
            Parent = entity.Parent != null ? Map(entity.Parent) : null,
            Level = entity.Level,
        };
    }
}
