namespace IAVH.BioTablero.CM.Application.Mappings.Initiatives;

using System;
using System.Linq;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Tags;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Entities.Tags;

/// <summary>
/// Initiative mappings.
/// </summary>
public class InitiativeMappings(
    IMapperCreateAndRead<InitiativeContact, InitiativeContactDto> initiativeContactMappings,
    IMapperCreateAndRead<InitiativeLocation, InitiativeLocationDto> initiativeLocationMappings,
    IMapperCreateAndRead<InitiativeUser, InitiativeUserDto> initiativeUserMappings,
    IMapperCreateAndRead<Tag, TagDto> tagMappings) : IMapperCreateAndRead<Initiative, InitiativeDto>
{
    /// <inheritdoc/>
    public InitiativeDto Map(Initiative entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            Name = entity.Name,
            ShortName = entity.ShortName,
            Description = entity.Description,
            Baseline = entity.Baseline,
            Objective = entity.Objective,
            CreationDate = entity.CreationDate,
            ImageUrl = entity.ImageUrl,
            BannerUrl = entity.BannerUrl,
            Enabled = entity.Enabled,
            Coordinate = [entity.Coordinate.X, entity.Coordinate.Y],
            PolygonArea = entity.PolygonArea,
            Contacts = entity.InitiativeContacts?.Select(initiativeContactMappings.Map),
            Locations = entity.InitiativeLocations?.Select(initiativeLocationMappings.Map),
            Users = entity.InitiativeUsers?.Select(initiativeUserMappings.Map),
            Tags = entity.InitiativeTags?.Select(e => tagMappings.Map(e.Tag)),
        };
    }

    /// <inheritdoc/>
    public Initiative Map(InitiativeDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            Name = dto.Name,
            ShortName = dto.ShortName,
            Description = dto.Description,
            Baseline = dto.Baseline,
            Objective = dto.Objective,
            CreationDate = dto.CreationDate ?? DateTime.Now,
            ImageUrl = dto.ImageUrl,
            BannerUrl = dto.BannerUrl,
            PolygonArea = dto.PolygonArea ?? 0,
            Enabled = dto?.Enabled ?? true,
            InitiativeContacts = [.. dto.Contacts?.Select(initiativeContactMappings.Map)],
            InitiativeLocations = [.. dto.Locations?.Select(initiativeLocationMappings.Map)],
            InitiativeUsers = [.. dto.Users?.Select(initiativeUserMappings.Map)],
        };
    }
}
