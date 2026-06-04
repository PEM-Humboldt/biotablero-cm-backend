namespace IAVH.BioTablero.CM.Application.Mappings.Indicators;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;
using IAVH.BioTablero.CM.Application.Mappings.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

/// <summary>
/// Measure Unit mappings.
/// </summary>
public class MeasureUnitMappings() : MapperRead<MeasureUnit, MeasureUnitDto>
{
    /// <inheritdoc/>
    public override MeasureUnitDto Map(MeasureUnit entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Representation = entity.Representation,
        };
    }
}
