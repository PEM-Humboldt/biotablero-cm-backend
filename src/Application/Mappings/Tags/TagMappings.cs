namespace IAVH.BioTablero.CM.Application.Mappings.Tags;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Tags;
using IAVH.BioTablero.CM.Application.DTOs.Utils;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Tags;

using TagCategoryEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.TagEnums.TagCategory;

/// <summary>
/// Tag mappings.
/// </summary>
public class TagMappings : IMapper<Tag, TagDto>
{
    /// <inheritdoc/>
    public TagDto Map(Tag entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Url = entity.Url.ToString(),
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
            Url = new Uri(dto.Url),
            CategoryId = dto.Category.Id,
        };
    }
}
