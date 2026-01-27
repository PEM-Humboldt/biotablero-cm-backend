namespace IAVH.BioTablero.CM.Application.Mappings.TerritoryStories;

using System;
using System.Linq;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;

/// <summary>
/// Territory Story mappings.
/// </summary>
public class TerritoryStoryMappings(
    IMapperCreateReadAndUpdate<TerritoryStoryImage, TerritoryStoryImageDto> territoryStoryImageMappings,
    IMapperCreateReadAndUpdate<TerritoryStoryVideo, TerritoryStoryVideoDto> territoryStoryVideoMappings) : IMapperCreateReadAndUpdate<TerritoryStory, TerritoryStoryDto>
{
    /// <inheritdoc/>
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
            ILikedIt = entity.ILikedIt,
            Images = entity.Images?.Select(territoryStoryImageMappings.Map),
            Videos = entity.Videos?.Select(territoryStoryVideoMappings.Map),
        };
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public void Update(TerritoryStory entity, TerritoryStoryDto dto)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(dto);

        entity.Title = dto.Title;
        entity.Text = dto.Text;
        entity.Keywords = dto.Keywords;
        entity.Restricted = dto.Restricted ?? entity.Restricted;
    }
}
