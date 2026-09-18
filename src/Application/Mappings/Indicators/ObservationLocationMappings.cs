namespace IAVH.BioTablero.CM.Application.Mappings.Indicators;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Geo;
using IAVH.BioTablero.CM.Application.DTOs.Indicators;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Mappings.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;
using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

/// <summary>
/// Observation Location mappings.
/// </summary>
public class ObservationLocationMappings(
    IMapperRead<Location, LocationDto> locationMappings) : MapperRead<ObservationLocation, ObservationLocationDto>
{
    /// <inheritdoc/>
    public override ObservationLocationDto Map(ObservationLocation? entity)
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
