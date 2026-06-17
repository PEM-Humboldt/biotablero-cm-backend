namespace IAVH.BioTablero.CM.Application.Mappings.Indicators;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;
using IAVH.BioTablero.CM.Application.DTOs.Tags;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Mappings.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;
using IAVH.BioTablero.CM.Core.Domain.Entities.Tags;

/// <summary>
/// Indicator Tag mappings.
/// </summary>
public class IndicatorTagMappings(IMapperCreateReadAndUpdate<Tag, TagDto> tagMappings) : MapperRead<IndicatorTag, IndicatorTagDto>
{
    /// <inheritdoc/>
    public override IndicatorTagDto Map(IndicatorTag entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            IndicatorTagId = entity.Id,
            Tag = tagMappings.Map(entity.Tag),
        };
    }
}
