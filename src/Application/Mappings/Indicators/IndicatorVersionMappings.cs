namespace IAVH.BioTablero.CM.Application.Mappings.Indicators;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;
using IAVH.BioTablero.CM.Application.Mappings.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

/// <summary>
/// Indicator Version mappings.
/// </summary>
public class IndicatorVersionMappings() : MapperRead<IndicatorVersion, IndicatorVersionDto>
{
    /// <inheritdoc/>
    public override IndicatorVersionDto Map(IndicatorVersion entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            IndicatorId = entity.IndicatorId,
            CreationDate = entity.CreationDate,
            Version = entity.Version,
            Description = entity.Description,
            Considerations = entity.Considerations,
            Interpretation = entity.Interpretation,
            Methodology = entity.Methodology,
            Authorship = entity.Authorship,
        };
    }
}
