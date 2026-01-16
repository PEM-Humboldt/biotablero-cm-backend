namespace IAVH.BioTablero.CM.Application.Mappings.Resources;

using System;
using System.Linq;

using IAVH.BioTablero.CM.Application.DTOs.Resources;
using IAVH.BioTablero.CM.Application.DTOs.Tags;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;
using IAVH.BioTablero.CM.Core.Domain.Entities.Tags;

/// <summary>
/// Resource mappings.
/// </summary>
public class ResourceMappings(
    IMapper<ResourceType, ResourceTypeDto> resourceTypeMappings,
    IMapper<ResourceFile, ResourceFileDto> resourceFileMappings,
    IMapper<ResourceLink, ResourceLinkDto> resourceLinkMappings,
    IMapper<Tag, TagDto> tagMappings) : IMapper<Resource, ResourceDto>
{
    /// <inheritdoc/>
    public ResourceDto Map(Resource entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            InitiativeId = entity.InitiativeId,
            Name = entity.Name,
            Description = entity.Description,
            CreationDate = entity.CreationDate,
            PublicationDate = entity.PublicationDate,
            IsDraft = entity.IsDraft,
            Likes = entity.TotalLikes,
            ILikedIt = entity.ILikedIt,
            ResourceType = resourceTypeMappings.Map(entity.ResourceType),
            Files = entity.Files?.Select(resourceFileMappings.Map),
            Links = entity.Links?.Select(resourceLinkMappings.Map),
            Tags = entity.ResourceTags?.Select(e => tagMappings.Map(e.Tag)),
        };
    }

    /// <inheritdoc/>
    public Resource Map(ResourceDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            Id = dto.Id ?? 0,
            InitiativeId = dto.InitiativeId,
            Name = dto.Name,
            Description = dto.Description,
            IsDraft = dto.IsDraft,
            ResourceType = resourceTypeMappings.Map(dto.ResourceType),
        };
    }
}
