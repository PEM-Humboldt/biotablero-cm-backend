namespace IAVH.BioTablero.CM.Application.Mappings;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Utils;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using InitiativeTagCategoryEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.InitiativeTagCategory;

/// <summary>
/// Initiative tag mappings.
/// </summary>
public class InitiativeTagMappings : IMapper<InitiativeTag, InitiativeTagDto>
{
    /// <summary>
    /// Map from entity to DTO.
    /// </summary>
    /// <param name="entity">Entity data.</param>
    /// <returns>DTO data.</returns>
    public InitiativeTagDto Map(InitiativeTag entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Url = entity.Url.ToString(),
            Category = new EnumEntityDto<InitiativeTagCategoryEnum>(entity.CategoryId),
        };
    }

    /// <summary>
    /// Map from DTO to entity.
    /// </summary>
    /// <param name="dto">DTO data.</param>
    /// <returns>Entity data.</returns>
    public InitiativeTag Map(InitiativeTagDto dto)
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
