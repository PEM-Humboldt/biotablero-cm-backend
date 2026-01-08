namespace IAVH.BioTablero.CM.Application.Mappings.Initiatives;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Geo;
using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Initiative contact mappings.
/// </summary>
public class InitiativeLocationMappings(
    IMapper<Location, LocationDto> locationMappings) : IMapper<InitiativeLocation, InitiativeLocationDto>
{
    /// <inheritdoc/>
    public InitiativeLocationDto Map(InitiativeLocation entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            LocationId = entity.LocationId,
            Locality = entity.Locality,
            Location = entity.Location != null ? locationMappings.Map(entity.Location) : null,
        };
    }

    /// <inheritdoc/>
    public InitiativeLocation Map(InitiativeLocationDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            Id = dto?.Id ?? 0,
            InitiativeId = dto.InitiativeId ?? 0,
            LocationId = dto.LocationId ?? 0,
            Locality = dto.Locality,
            Location = dto.Location != null ? locationMappings.Map(dto.Location) : null,
        };
    }
}
