namespace IAVH.BioTablero.CM.Application.Mappings.Indicators;

using System;
using System.Linq;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Mappings.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

/// <summary>
/// Indicator mappings.
/// </summary>
public class ObservationMappings(
    IMapperRead<IndicatorTag, IndicatorTagDto> observationTagMappings,
    IMapperRead<IndicatorTopic, IndicatorTopicDto> observationTopicMappings,
    IMapperRead<IndicatorLocation, IndicatorLocationDto> observationLocationMappings) : MapperRead<Observation, ObservationDto>, IMapperReadAndUpdate<Observation, ObservationDto>
{
    /// <inheritdoc/>
    public override ObservationDto Map(Observation? entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            Name = entity.Name,
            InitiativeId = entity.InitiativeId,
            InitiativeName = entity.Initiative?.Name,
            Topic = entity.Topic != null ? observationTopicMappings.Map(entity.Topic) : null,
            Tags = entity.ObservationTags?.Select(observationTagMappings.Map),
            Locations = entity.ObservationLocations?.Select(observationLocationMappings.Map),
            Versions = entity.Versions?
                .Select(v => new IndicatorVersionDto()
                {
                    Id = v.Id,
                    Version = v.Version,
                    CreationDate = v.CreationDate,
                })
                .ToList(),
        };
    }

    /// <inheritdoc/>
    public void Update(Observation entity, ObservationDto dto)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(dto);

        entity.Name = dto.Name;
    }
}
