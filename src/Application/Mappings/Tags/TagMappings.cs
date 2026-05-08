namespace IAVH.BioTablero.CM.Application.Mappings.Tags;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Tags;
using IAVH.BioTablero.CM.Application.DTOs.Utils;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Mappings.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Tags;

using TagCategoryEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.TagEnums.TagCategory;

/// <summary>
/// Tag mappings.
/// </summary>
public class TagMappings : MapperRead<Tag, TagDto>, IMapperCreateReadAndUpdate<Tag, TagDto>
{
    /// <inheritdoc/>
    public override TagDto Map(Tag entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Url = entity.Url?.ToString(),
            Category = new EnumEntityDto<TagCategoryEnum>(entity.CategoryId),
        };
    }

    /// <inheritdoc/>
    public Tag Map(TagDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            Name = dto.Name,
            Url = dto.Url != null ? new Uri(dto.Url) : null,
            CategoryId = dto.Category.Id,
        };
    }

    /// <inheritdoc/>
    public void Update(Tag entity, TagDto dto)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(dto);

        entity.Name = dto.Name;
        entity.Url = dto.Url != null ? new Uri(dto.Url) : null;
    }
}
