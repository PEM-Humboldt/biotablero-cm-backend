namespace IAVH.BioTablero.CM.Application.Mappings.Indicators;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;
using IAVH.BioTablero.CM.Application.Mappings.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

/// <summary>
/// Category mappings.
/// </summary>
public class CategoryMappings() : MapperRead<Category, CategoryDto>
{
    /// <inheritdoc/>
    public override CategoryDto Map(Category entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Parent = entity.Parent != null ? Map(entity.Parent) : null,
        };
    }
}
