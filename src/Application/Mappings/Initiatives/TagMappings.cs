namespace IAVH.BioTablero.CM.Application.Mappings.Initiatives;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Utils;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using TagCategoryEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.TagCategory;

/// <summary>
/// Tag mappings.
/// </summary>
public class TagMappings : IMapper<Tag, TagDto>
{
    /// <summary>
    /// Map from entity to DTO.
    /// </summary>
    /// <param name="entity">Entity data.</param>
    /// <returns>DTO data.</returns>
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

    /// <summary>
    /// Map from DTO to entity.
    /// </summary>
    /// <param name="dto">DTO data.</param>
    /// <returns>Entity data.</returns>
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
