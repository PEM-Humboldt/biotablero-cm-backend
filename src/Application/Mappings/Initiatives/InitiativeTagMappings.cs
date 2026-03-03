namespace IAVH.BioTablero.CM.Application.Mappings.Initiatives;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Tags;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Entities.Tags;

/// <summary>
/// Resource Tag mappings.
/// </summary>
public class InitiativeTagMappings(IMapperCreateReadAndUpdate<Tag, TagDto> tagMappings) : IMapperRead<InitiativeTag, InitiativeTagDto>
{
    /// <inheritdoc/>
    public InitiativeTagDto Map(InitiativeTag entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            InitiativeTagId = entity.Id,
            Tag = tagMappings.Map(entity.Tag),
        };
    }
}
