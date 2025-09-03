namespace IAVH.BioTablero.CM.Application.Mappings;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Geo;
using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Initiative contact mappings.
/// </summary>
public class InitiativeLocationMappings : IMapper<InitiativeLocation, InitiativeLocationDto>
{
    /// <summary>
    /// Map from entity to DTO.
    /// </summary>
    /// <param name="entity">Entity data.</param>
    /// <returns>DTO data.</returns>
    public InitiativeLocationDto Map(InitiativeLocation entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            LocationId = entity.LocationId,
            Locality = entity.Locality,
            Location = new LocationDto()
            {
                Id = entity.Location?.Id,
                Code = entity.Location?.Code,
                Name = entity.Location?.Name,
            },
        };
    }

    /// <summary>
    /// Map from DTO to entity.
    /// </summary>
    /// <param name="dto">DTO data.</param>
    /// <returns>Entity data.</returns>
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
}
