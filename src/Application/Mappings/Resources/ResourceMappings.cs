namespace IAVH.BioTablero.CM.Application.Mappings.Resources;

using System;
using System.Linq;

using IAVH.BioTablero.CM.Application.DTOs.Resources;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Mappings.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;

/// <summary>
/// Resource mappings.
/// </summary>
public class ResourceMappings(
    IMapperRead<ResourceType, ResourceTypeDto> resourceTypeMappings,
    IMapperCreateReadAndUpdate<ResourceFile, ResourceFileDto> resourceFileMappings,
    IMapperCreateReadAndUpdate<ResourceLink, ResourceLinkDto> resourceLinkMappings,
    IMapperRead<ResourceTag, ResourceTagDto> resourceTagMappings) : MapperRead<Resource, ResourceDto>, IMapperCreateReadAndUpdate<Resource, ResourceDto>
{
    /// <inheritdoc/>
    public override ResourceDto Map(Resource entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            InitiativeId = entity.InitiativeId,
            AuthorUserName = entity.AuthorUserName,
            Name = entity.Name,
            Description = entity.Description,
            CreationDate = entity.CreationDate.ToUniversalTime(),
            PublicationDate = entity.PublicationDate?.ToUniversalTime(),
            IsDraft = entity.IsDraft,
            Likes = entity.TotalLikes,
            ILikedIt = entity.ILikedIt,
            ResourceType = entity.ResourceType != null ? resourceTypeMappings.Map(entity.ResourceType) : null,
            Files = entity.Files?.Select(resourceFileMappings.Map),
            Links = entity.Links?.Select(resourceLinkMappings.Map),
            Tags = entity.ResourceTags?.Select(resourceTagMappings.Map),
        };
    }

    /// <inheritdoc/>
    public override ResourceDto MapOdata(Resource entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            InitiativeId = entity.InitiativeId,
            AuthorUserName = entity.AuthorUserName,
            Name = entity.Name,
            CreationDate = entity.CreationDate.ToUniversalTime(),
            PublicationDate = entity.PublicationDate?.ToUniversalTime(),
            IsDraft = entity.IsDraft,
            Likes = entity.TotalLikes,
            ILikedIt = entity.ILikedIt,
            TotalFiles = entity.TotalFiles,
            TotalLinks = entity.TotalLinks,
            ResourceType = entity.ResourceType != null ? resourceTypeMappings.Map(entity.ResourceType) : null,
            Tags = entity.ResourceTags?.Select(resourceTagMappings.Map),
        };
    }

    /// <inheritdoc/>
    public Resource Map(ResourceDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            Id = dto.Id ?? 0,
            InitiativeId = dto.InitiativeId ?? 0,
            AuthorUserName = dto.AuthorUserName,
            ResourceTypeId = dto.ResourceType.Id ?? 0,
            Name = dto.Name,
            Description = dto.Description,
            IsDraft = dto.IsDraft,
        };
    }

    /// <inheritdoc/>
    public void Update(Resource entity, ResourceDto dto)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(dto);

        entity.ResourceTypeId = dto.ResourceType.Id.Value;
        entity.Name = dto.Name;
        entity.Description = dto.Description;
        entity.IsDraft = dto.IsDraft;

        if (!entity.IsDraft)
        {
            entity.PublicationDate = DateTimeOffset.UtcNow;
        }
    }
}
