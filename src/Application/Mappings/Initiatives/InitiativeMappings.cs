namespace IAVH.BioTablero.CM.Application.Mappings.Initiatives;

using System;
using System.Linq;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Initiative mappings.
/// </summary>
public class InitiativeMappings(
    IMapperCreateReadAndUpdate<InitiativeContact, InitiativeContactDto> initiativeContactMappings,
    IMapperCreateReadAndUpdate<InitiativeLocation, InitiativeLocationDto> initiativeLocationMappings,
    IMapperCreateReadAndUpdate<InitiativeUser, InitiativeUserDto> initiativeUserMappings,
    IMapperRead<InitiativeTag, InitiativeTagDto> initiativeTagMappings) : IMapperCreateReadAndUpdate<Initiative, InitiativeDto>
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
            Coordinate = [entity.Coordinate.Y, entity.Coordinate.X],
            PolygonArea = entity.PolygonArea,
            Contacts = entity.InitiativeContacts?.Select(initiativeContactMappings.Map),
            Locations = entity.InitiativeLocations?.Select(initiativeLocationMappings.Map),
            Users = entity.InitiativeUsers?.Select(initiativeUserMappings.Map),
            Tags = entity.InitiativeTags?.Select(initiativeTagMappings.Map),
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

    /// <inheritdoc/>
    public void Update(Initiative entity, InitiativeDto dto)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(dto);

        entity.Name = dto.Name;
        entity.ShortName = dto.ShortName;
        entity.Description = dto.Description;
        entity.Baseline = dto.Baseline;
        entity.Objective = dto.Objective;
    }
}
