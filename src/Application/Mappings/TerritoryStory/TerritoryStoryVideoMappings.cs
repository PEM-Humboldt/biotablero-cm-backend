namespace IAVH.BioTablero.CM.Application.Mappings.TerritoryStory;

using System;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;

/// <summary>
/// Territory Story Video mappings.
/// </summary>
public class TerritoryStoryVideoMappings : IMapper<TerritoryStoryVideo, TerritoryStoryVideoDto>
{
    /// <summary>
    /// Map from entity to DTO.
    /// </summary>
    /// <param name="entity">Entity data.</param>
    /// <returns>DTO data.</returns>
    public TerritoryStoryVideoDto Map(TerritoryStoryVideo entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            TerritoryStoryId = entity.TerritoryStoryId,
            FileUrl = entity.FileUrl.ToString(),
        };
    }

    /// <summary>
    /// Map from DTO to entity.
    /// </summary>
    /// <param name="dto">DTO data.</param>
    /// <returns>Entity data.</returns>
    public TerritoryStoryVideo Map(TerritoryStoryVideoDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            Id = dto.Id ?? 0,
            TerritoryStoryId = dto.TerritoryStoryId ?? 0,
            FileUrl = new Uri(dto.FileUrl),
        };
    }
}
