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
public class IndicatorMappings(
    IMapperRead<IndicatorTag, IndicatorTagDto> indicatorTagMappings,
    IMapperRead<IndicatorType, IndicatorTypeDto> indicatorTypeMappings,
    IMapperRead<IndicatorLocation, IndicatorLocationDto> indicatorLocationMappings) : MapperRead<Indicator, IndicatorDto>
{
    /// <inheritdoc/>
    public override IndicatorDto Map(Indicator entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            InitiativeId = entity.InitiativeId,
            Type = indicatorTypeMappings.Map(entity.Type),
            Tags = entity.IndicatorTags?.Select(indicatorTagMappings.Map),
            Locations = entity.IndicatorLocations?.Select(indicatorLocationMappings.Map),
            Versions = [.. entity.Versions.Select(v =>
                new IndicatorVersionDto()
                {
                    Id = v.Id,
                    Version = v.Version,
                })
            ],
        };
    }
}
