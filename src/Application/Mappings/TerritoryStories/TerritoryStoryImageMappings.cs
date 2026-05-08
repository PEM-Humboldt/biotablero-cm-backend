namespace IAVH.BioTablero.CM.Application.Mappings.TerritoryStories;

using System;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Mappings.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;

/// <summary>
/// Territory Story Image mappings.
/// </summary>
public class TerritoryStoryImageMappings : MapperRead<TerritoryStoryImage, TerritoryStoryImageDto>, IMapperCreateReadAndUpdate<TerritoryStoryImage, TerritoryStoryImageDto>
{
    /// <inheritdoc/>
    public override TerritoryStoryImageDto Map(TerritoryStoryImage entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            TerritoryStoryId = entity.TerritoryStoryId,
            FileUrl = entity.FileUrl,
            Description = entity.Description,
            FeaturedContent = entity.FeaturedContent,
        };
    }

    /// <inheritdoc/>
    public TerritoryStoryImage Map(TerritoryStoryImageDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            Id = dto.Id ?? 0,
            TerritoryStoryId = dto.TerritoryStoryId ?? 0,
            FileUrl = dto.FileUrl,
            Description = dto.Description,
            FeaturedContent = dto.FeaturedContent,
        };
    }

    /// <inheritdoc/>
    public void Update(TerritoryStoryImage entity, TerritoryStoryImageDto dto)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(dto);

        entity.Description = dto.Description;
    }
}
