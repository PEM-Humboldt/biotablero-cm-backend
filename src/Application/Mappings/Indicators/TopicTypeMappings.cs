namespace IAVH.BioTablero.CM.Application.Mappings.Indicators;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;
using IAVH.BioTablero.CM.Application.Mappings.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

/// <summary>
/// Topic Type mappings.
/// </summary>
public class TopicTypeMappings() : MapperRead<TopicType, TopicTypeDto>
{
    /// <inheritdoc/>
    public override TopicTypeDto Map(TopicType? entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
    }
}
