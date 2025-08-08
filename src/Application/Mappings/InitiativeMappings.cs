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
    IMapper<InitiativeUser, InitiativeUserDto> initiativeUserMappings) : IMapper<Initiative, InitiativeDto>
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
            LogoUrl = entity.LogoUrl,
            Enabled = entity.Enabled,
            InitiativeContacts = entity.InitiativeContacts?.Select(initiativeContactMappings.Map),
            InitiativeLocations = entity.InitiativeLocations?.Select(initiativeLocationMappings.Map),
            InitiativeUsers = entity.InitiativeUsers?.Select(initiativeUserMappings.Map),
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
            LogoUrl = dto.LogoUrl,
            Enabled = dto?.Enabled ?? true,
            InitiativeContacts = [.. dto.InitiativeContacts?.Select(initiativeContactMappings.Map)],
            InitiativeLocations = [.. dto.InitiativeLocations?.Select(initiativeLocationMappings.Map)],
            InitiativeUsers = [.. dto.InitiativeUsers?.Select(initiativeUserMappings.Map)],
        };
    }
}
