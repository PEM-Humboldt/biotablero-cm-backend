namespace IAVH.BioTablero.CM.Application.Mappings.Indicators;

using System;
using System.Linq;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Mappings.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

/// <summary>
/// Observation Version mappings.
/// </summary>
public class ObservationVersionMappings(
    IMapperRead<Category, CategoryDto> categoryMappings,
    IMapperRead<IndicatorValue, IndicatorValueDto> indicatorValueMappings) : MapperRead<TopicVersion, TopicVersionDto>, IMapperReadAndUpdate<TopicVersion, TopicVersionDto>
{
    /// <inheritdoc/>
    public override TopicVersionDto Map(TopicVersion? entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            TopicId = entity.TopicId,
            CreationDate = entity.CreationDate,
            Version = entity.Version,
            Description = entity.Description,
            Considerations = entity.Considerations,
            Interpretation = entity.Interpretation,
            Methodology = entity.Methodology,
            Authorship = entity.Authorship,
            Groups = entity.Groups?.Select(e => new TopicGroupDto()
            {
                Id = e.Id,
                Category = categoryMappings.Map(e.Category),
                Values = e.Values?.Select(indicatorValueMappings.Map),
            }),
        };
    }

    /// <inheritdoc/>
    public void Update(TopicVersion entity, TopicVersionDto dto)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(dto);

        entity.Description = dto.Description;
        entity.Methodology = dto.Methodology;
        entity.Interpretation = dto.Interpretation;
        entity.Considerations = dto.Considerations;
        entity.Authorship = dto.Authorship;
    }
}
