namespace IAVH.BioTablero.CM.Application.Mappings.Indicators;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;
using IAVH.BioTablero.CM.Application.DTOs.Tags;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Mappings.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;
using IAVH.BioTablero.CM.Core.Domain.Entities.Tags;

/// <summary>
/// Topic Tag mappings.
/// </summary>
public class TopicTagMappings(IMapperCreateReadAndUpdate<Tag, TagDto> tagMappings) : MapperRead<TopicTag, TopicTagDto>
{
    /// <inheritdoc/>
    public override TopicTagDto Map(TopicTag? entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            TopicTagId = entity.Id,
            Tag = tagMappings.Map(entity.Tag),
        };
    }
}
