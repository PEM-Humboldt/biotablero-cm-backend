namespace IAVH.BioTablero.CM.Application.Mappings.TerritoryStory;

using System;
using System.Linq;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;

/// <summary>
/// Territory Story mappings.
/// </summary>
public class TerritoryStoryMappings(
    IMapper<TerritoryStoryImage, TerritoryStoryImageDto> territoryStoryImageMappings,
    IMapper<TerritoryStoryVideo, TerritoryStoryVideoDto> territoryStoryVideoMappings) : IMapper<TerritoryStory, TerritoryStoryDto>
{
    /// <summary>
    /// Map from entity to DTO.
    /// </summary>
    /// <param name="entity">Entity data.</param>
    /// <returns>DTO data.</returns>
    public TerritoryStoryDto Map(TerritoryStory entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            InitiativeId = entity.InitiativeId,
            AuthorUserName = entity.AuthorUserName,
            CreationDate = entity.CreationDate,
            Title = entity.Title,
            Text = entity.Text,
            Keywords = entity.Keywords,
            Enabled = entity.Enabled,
            FeaturedContent = entity.FeaturedContent,
            Restricted = entity.Restricted,
            Likes = entity.TotalLikes,
            Images = entity.Images?.Select(territoryStoryImageMappings.Map),
            Videos = entity.Videos?.Select(territoryStoryVideoMappings.Map),
        };
    }

    /// <summary>
    /// Map from DTO to entity.
    /// </summary>
    /// <param name="dto">DTO data.</param>
    /// <returns>Entity data.</returns>
    public TerritoryStory Map(TerritoryStoryDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            Id = dto.Id ?? 0,
            InitiativeId = dto.InitiativeId ?? 0,
            AuthorUserName = dto.AuthorUserName,
            CreationDate = dto.CreationDate ?? DateTime.Now,
            Title = dto.Title,
            Text = dto.Text,
            Keywords = dto.Keywords,
            Enabled = dto.Enabled ?? true,
            FeaturedContent = dto.FeaturedContent ?? false,
            Restricted = dto.Restricted ?? false,
            Images = dto.Images?.Select(territoryStoryImageMappings.Map).ToList(),
            Videos = dto.Videos?.Select(territoryStoryVideoMappings.Map).ToList(),
        };
    }
}
