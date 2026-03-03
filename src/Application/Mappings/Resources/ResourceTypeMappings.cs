namespace IAVH.BioTablero.CM.Application.Mappings.Resources;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Resources;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;

/// <summary>
/// Resource Type mappings.
/// </summary>
public class ResourceTypeMappings : IMapperRead<ResourceType, ResourceTypeDto>
{
    /// <inheritdoc/>
    public ResourceTypeDto Map(ResourceType entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
        };
    }
}
