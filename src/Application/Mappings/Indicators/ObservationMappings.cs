namespace IAVH.BioTablero.CM.Application.Mappings.Indicators;

using System;
using System.Linq;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Mappings.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

/// <summary>
/// Observation mappings.
/// </summary>
public class ObservationMappings(
    IMapperRead<TopicTag, TopicTagDto> observationTagMappings,
    IMapperRead<TopicType, TopicTypeDto> observationTopicMappings,
    IMapperRead<TopicLocation, TopicLocationDto> observationLocationMappings) : MapperRead<Topic, TopicDto>, IMapperReadAndUpdate<Topic, TopicDto>
{
    /// <inheritdoc/>
    public override TopicDto Map(Topic? entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            Name = entity.Name,
            InitiativeId = entity.InitiativeId,
            InitiativeName = entity.Initiative?.Name,
            TopicType = entity.TopicType != null ? observationTopicMappings.Map(entity.TopicType) : null,
            Tags = entity.TopicTags?.Select(observationTagMappings.Map),
            Locations = entity.TopicLocations?.Select(observationLocationMappings.Map),
            Versions = entity.Versions?
                .Select(v => new TopicVersionDto()
                {
                    Id = v.Id,
                    Version = v.Version,
                    CreationDate = v.CreationDate,
                })
                .ToList(),
        };
    }

    /// <inheritdoc/>
    public void Update(Topic entity, TopicDto dto)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(dto);

        entity.Name = dto.Name;
    }
}
