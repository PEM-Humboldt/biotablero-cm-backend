namespace IAVH.BioTablero.CM.Application.Mappings;

using System;
using System.Linq;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Initiative mappings.
/// </summary>
public class InitiativeMappings(
    IMapper<InitiativeContact, InitiativeContactDto> initiativeContactMappings,
    IMapper<InitiativeLocation, InitiativeLocationDto> initiativeLocationMappings,
    IMapper<InitiativeUser, InitiativeUserDto> initiativeUserMappings,
    IMapper<InitiativeTag, InitiativeTagDto> initiativeTagMappings) : IMapper<Initiative, InitiativeDto>
{
    /// <summary>
    /// Map from entity to DTO.
    /// </summary>
    /// <param name="entity">Entity data.</param>
    /// <returns>DTO data.</returns>
    public InitiativeDto Map(Initiative entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            CreationDate = entity.CreationDate,
            ImageUrl = entity.ImageUrl,
            BannerUrl = entity.BannerUrl,
            Enabled = entity.Enabled,
            Coordinate = [entity.Coordinate.X, entity.Coordinate.Y],
            Contacts = entity.InitiativeContacts?.Select(initiativeContactMappings.Map),
            Locations = entity.InitiativeLocations?.Select(initiativeLocationMappings.Map),
            Users = entity.InitiativeUsers?.Select(initiativeUserMappings.Map),
            Tags = entity.InitiativeTagInitiatives?.Select(e => initiativeTagMappings.Map(e.Tag)),
        };
    }

    /// <summary>
    /// Map from DTO to entity.
    /// </summary>
    /// <param name="dto">DTO data.</param>
    /// <returns>Entity data.</returns>
    public Initiative Map(InitiativeDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            Name = dto.Name,
            Description = dto.Description,
            CreationDate = dto.CreationDate ?? DateTime.Now,
            ImageUrl = dto.ImageUrl,
            BannerUrl = dto.BannerUrl,
            Enabled = dto?.Enabled ?? true,
            InitiativeContacts = [.. dto.Contacts?.Select(initiativeContactMappings.Map)],
            InitiativeLocations = [.. dto.Locations?.Select(initiativeLocationMappings.Map)],
            InitiativeUsers = [.. dto.Users?.Select(initiativeUserMappings.Map)],
        };
    }
}
