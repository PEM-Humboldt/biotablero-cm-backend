namespace IAVH.BioTablero.CM.Application.Mappings;

using System;
using System.Linq;

using IAVH.BioTablero.CM.Application.DTOs.Geo;
using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Initiative mappings
/// </summary>
public class InitiativeMappings : IMapper<Initiative, InitiativeDto>
{
    /// <summary>
    /// Map from entity to DTO
    /// </summary>
    /// <param name="entity">Entity data</param>
    /// <returns>DTO data</returns>
    public InitiativeDto Map(Initiative entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            LogoUrl = entity.LogoUrl,
            InitiativeContacts = entity.InitiativeContacts.Select(i => new InitiativeContactDto()
            {
                Id = i.Id,
                InitiativeId = i.InitiativeId,
                Phone = i.Phone,
                Email = i.Email,
            }),
            InitiativeLocations = entity.InitiativeLocations.Select(i => new InitiativeLocationDto()
            {
                Id = i.Id,
                Locality = i.Locality,
                Location = new LocationDto()
                {
                    Id = i.LocationId,
                    Code = i.Location.Code,
                    Name = i.Location.Name,
                },
            }),
            // InitiativeUsers = entity.InitiativeUsers.Select(i => new InitiativeUserDto()
            // {
            //     Id = i.Id,
            // }),
        };
    }

    /// <summary>
    /// Map from DTO to entity
    /// </summary>
    /// <param name="dto">DTO data</param>
    /// <returns>Entity data</returns>
    public Initiative Map(InitiativeDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            LogoUrl = dto.LogoUrl,
        };
    }
}
