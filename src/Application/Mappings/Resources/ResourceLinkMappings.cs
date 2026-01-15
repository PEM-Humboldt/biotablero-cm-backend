namespace IAVH.BioTablero.CM.Application.Mappings.Resources;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Resources;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;

/// <summary>
/// Resource Link mappings.
/// </summary>
public class ResourceLinkMappings : IMapper<ResourceLink, ResourceLinkDto>
{
    /// <inheritdoc/>
    public ResourceLinkDto Map(ResourceLink entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            ResourceId = entity.ResourceId,
            Url = entity.Url.ToString(),
            Name = entity.Name,
        };
    }

    /// <inheritdoc/>
    public ResourceLink Map(ResourceLinkDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            Id = dto.Id ?? 0,
            ResourceId = dto.ResourceId,
            Url = new Uri(dto.Url),
            Name = dto.Name,
        };
    }
}
