namespace IAVH.BioTablero.CM.Application.Mappings.Initiatives;

using System;
using System.Linq;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Mappings.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using NetTopologySuite.Geometries;

/// <summary>
/// Initiative mappings.
/// </summary>
public class InitiativeMappings(
    IMapperCreateReadAndUpdate<InitiativeContact, InitiativeContactDto> initiativeContactMappings,
    IMapperCreateReadAndUpdate<InitiativeLocation, InitiativeLocationDto> initiativeLocationMappings,
    IMapperCreateReadAndUpdate<InitiativeUser, InitiativeUserDto> initiativeUserMappings,
    IMapperRead<InitiativeTag, InitiativeTagDto> initiativeTagMappings) : MapperRead<Initiative, InitiativeDto>, IMapperCreateReadAndUpdate<Initiative, InitiativeDto>
{
    /// <inheritdoc/>
    public override InitiativeDto Map(Initiative? entity)
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
            CreationDate = entity.CreationDate.ToUniversalTime(),
            ImageUrl = entity.ImageUrl,
            BannerUrl = entity.BannerUrl,
            Coordinate = entity.Coordinate is Point point && !point.IsEmpty
                ? [point.Y, point.X]
                : null,
            MainLocationId = entity.MainLocationId,
            PolygonArea = entity.PolygonArea,
            Enabled = entity.Enabled,
            HasPolygon = entity.Polygon != null,
            Contacts = entity.InitiativeContacts?.Select(initiativeContactMappings.Map),
            Locations = entity.InitiativeLocations?.Select(initiativeLocationMappings.Map),
            Users = entity.InitiativeUsers?.Select(initiativeUserMappings.Map),
            Tags = entity.InitiativeTags?.Select(initiativeTagMappings.Map),
        };
    }

    /// <inheritdoc/>
    public Initiative Map(InitiativeDto? dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            Name = dto.Name,
            ShortName = dto.ShortName,
            Description = dto.Description,
            Baseline = dto.Baseline,
            Objective = dto.Objective,
            CreationDate = dto.CreationDate ?? DateTimeOffset.UtcNow,
            ImageUrl = dto.ImageUrl,
            BannerUrl = dto.BannerUrl,
            Enabled = dto.Enabled ?? true,
            Coordinate = Point.Empty,
            InitiativeContacts = dto.Contacts?.Select(initiativeContactMappings.Map).ToList(),
            InitiativeLocations = dto.Locations?.Select(initiativeLocationMappings.Map).ToList(),
            InitiativeUsers = dto.Users?.Select(initiativeUserMappings.Map).ToList(),
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
