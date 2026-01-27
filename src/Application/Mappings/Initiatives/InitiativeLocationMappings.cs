namespace IAVH.BioTablero.CM.Application.Mappings.Initiatives;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Geo;
using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Initiative contact mappings.
/// </summary>
public class InitiativeLocationMappings(
    IMapperRead<Location, LocationDto> locationMappings) : IMapperCreateReadAndUpdate<InitiativeLocation, InitiativeLocationDto>
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
        };
    }

    /// <inheritdoc/>
    public void Update(InitiativeLocation entity, InitiativeLocationDto dto)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(dto);

        entity.LocationId = dto.LocationId ?? 0;
        entity.Locality = dto.Locality;
    }
}
