namespace IAVH.BioTablero.CM.Application.Services.Geo;

using IAVH.BioTablero.CM.Application.DTOs.Geo;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Specifications;
using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

/// <summary>
/// Location service
/// </summary>
public class LocationService(IRepository<Location> entityRepository,
    IMapper<Location, LocationDto> mapper) : ServiceRead<Location, LocationDto, int, LocationSpec>(entityRepository, mapper), ILocationService
{
}
