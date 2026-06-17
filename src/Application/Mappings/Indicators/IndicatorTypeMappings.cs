namespace IAVH.BioTablero.CM.Application.Mappings.Indicators;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;
using IAVH.BioTablero.CM.Application.Mappings.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

/// <summary>
/// Indicator Type mappings.
/// </summary>
public class IndicatorTypeMappings() : MapperRead<IndicatorType, IndicatorTypeDto>
{
    /// <inheritdoc/>
    public override IndicatorTypeDto Map(IndicatorType entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
    }
}
