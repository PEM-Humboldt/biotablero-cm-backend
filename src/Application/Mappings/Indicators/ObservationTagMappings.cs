namespace IAVH.BioTablero.CM.Application.Mappings.Indicators;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;
using IAVH.BioTablero.CM.Application.DTOs.Tags;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Mappings.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;
using IAVH.BioTablero.CM.Core.Domain.Entities.Tags;

/// <summary>
/// Observation Tag mappings.
/// </summary>
public class ObservationTagMappings(IMapperCreateReadAndUpdate<Tag, TagDto> tagMappings) : MapperRead<ObservationTag, ObservationTagDto>
{
    /// <inheritdoc/>
    public override ObservationTagDto Map(ObservationTag? entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            ObservationTagId = entity.Id,
            Tag = tagMappings.Map(entity.Tag),
        };
    }
}
