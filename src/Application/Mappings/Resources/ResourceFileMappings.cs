namespace IAVH.BioTablero.CM.Application.Mappings.Resources;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Resources;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;

/// <summary>
/// Resource File mappings.
/// </summary>
public class ResourceFileMappings : IMapperCreateAndRead<ResourceFile, ResourceFileDto>
{
    /// <inheritdoc/>
    public ResourceFileDto Map(ResourceFile entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            ResourceId = entity.ResourceId,
            Url = entity.Url,
            Name = entity.Name,
        };
    }

    /// <inheritdoc/>
    public ResourceFile Map(ResourceFileDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            Id = dto.Id ?? 0,
            ResourceId = dto.ResourceId,
            Url = dto.Url,
            Name = dto.Name,
        };
    }
}
