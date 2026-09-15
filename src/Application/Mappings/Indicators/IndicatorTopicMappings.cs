namespace IAVH.BioTablero.CM.Application.Mappings.Indicators;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;
using IAVH.BioTablero.CM.Application.Mappings.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

/// <summary>
/// Indicator Topic mappings.
/// </summary>
public class IndicatorTopicMappings() : MapperRead<IndicatorTopic, IndicatorTopicDto>
{
    /// <inheritdoc/>
    public override IndicatorTopicDto Map(IndicatorTopic? entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
    }
}
