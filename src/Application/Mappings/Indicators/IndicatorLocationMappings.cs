namespace IAVH.BioTablero.CM.Application.Mappings.Indicators;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Geo;
using IAVH.BioTablero.CM.Application.DTOs.Indicators;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Mappings.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;
using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

/// <summary>
/// Indicator Location mappings.
/// </summary>
public class IndicatorLocationMappings(
    IMapperRead<Location, LocationDto> locationMappings) : MapperRead<IndicatorLocation, IndicatorLocationDto>
{
    /// <inheritdoc/>
    public override IndicatorLocationDto Map(IndicatorLocation entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            LocationId = entity.LocationId,
            Locality = entity.Locality,
            Location = entity.Location != null ? locationMappings.Map(entity.Location) : null,
        };
    }
}
