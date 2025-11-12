namespace IAVH.BioTablero.CM.Application.Mappings.TerritoryStory;

using System;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;

/// <summary>
/// Territory Story Image mappings.
/// </summary>
public class TerritoryStoryImageMappings : IMapper<TerritoryStoryImage, TerritoryStoryImageDto>
{
    /// <summary>
    /// Map from entity to DTO.
    /// </summary>
    /// <param name="entity">Entity data.</param>
    /// <returns>DTO data.</returns>
    public TerritoryStoryImageDto Map(TerritoryStoryImage entity)
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

    /// <summary>
    /// Map from DTO to entity.
    /// </summary>
    /// <param name="dto">DTO data.</param>
    /// <returns>Entity data.</returns>
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
}
