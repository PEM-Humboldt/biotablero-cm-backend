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
    IMapperRead<IndicatorValue, IndicatorValueDto> indicatorValueMappings) : MapperRead<ObservationVersion, ObservationVersionDto>, IMapperReadAndUpdate<ObservationVersion, ObservationVersionDto>
{
    /// <inheritdoc/>
    public override ObservationVersionDto Map(ObservationVersion? entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            ObservationId = entity.ObservationId,
            CreationDate = entity.CreationDate,
            Version = entity.Version,
            Description = entity.Description,
            Considerations = entity.Considerations,
            Interpretation = entity.Interpretation,
            Methodology = entity.Methodology,
            Authorship = entity.Authorship,
            Groups = entity.Groups?.Select(e => new IndicatorGroupDto()
            {
                Id = e.Id,
                Category = categoryMappings.Map(e.Category),
                Values = e.Values?.Select(indicatorValueMappings.Map),
            }),
        };
    }

    /// <inheritdoc/>
    public void Update(ObservationVersion entity, ObservationVersionDto dto)
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
