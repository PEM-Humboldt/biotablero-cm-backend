namespace IAVH.BioTablero.CM.Application.Mappings.TerritoryStories;

using System;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;

/// <summary>
/// Territory Story Video mappings.
/// </summary>
public class TerritoryStoryVideoMappings : IMapperCreateReadAndUpdate<TerritoryStoryVideo, TerritoryStoryVideoDto>
{
    /// <inheritdoc/>
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

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public void Update(TerritoryStoryVideo entity, TerritoryStoryVideoDto dto)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(dto);

        entity.FileUrl = new Uri(dto.FileUrl);
    }
}
