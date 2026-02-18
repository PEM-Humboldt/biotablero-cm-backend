namespace IAVH.BioTablero.CM.Application.Mappings.Resources;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Resources;
using IAVH.BioTablero.CM.Application.DTOs.Tags;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;
using IAVH.BioTablero.CM.Core.Domain.Entities.Tags;

/// <summary>
/// Resource Tag mappings.
/// </summary>
public class ResourceTagMappings(IMapper<Tag, TagDto> tagMappings) : IMapper<ResourceTag, ResourceTagDto>
{
    /// <inheritdoc/>
    public ResourceTagDto Map(ResourceTag entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            ResourceTagId = entity.Id,
            Tag = tagMappings.Map(entity.Tag),
        };
    }

    /// <inheritdoc/>
    public ResourceTag Map(ResourceTagDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
        };
    }
}
