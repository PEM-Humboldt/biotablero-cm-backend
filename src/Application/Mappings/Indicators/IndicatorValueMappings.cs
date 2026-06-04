namespace IAVH.BioTablero.CM.Application.Mappings.Indicators;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Mappings.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

/// <summary>
/// Indicator Value mappings.
/// </summary>
public class IndicatorValueMappings(
    IMapperRead<MeasureUnit, MeasureUnitDto> measureUnitMappings) : MapperRead<IndicatorValue, IndicatorValueDto>
{
    /// <inheritdoc/>
    public override IndicatorValueDto Map(IndicatorValue entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            Date = entity.Date,
            DateEnd = entity.DateEnd,
            Value = entity.Value,
            UpperLimit = entity.UpperLimit,
            LowerLimit = entity.LowerLimit,
            MeasureUnit = measureUnitMappings.Map(entity.MeasureUnit),
        };
    }
}
