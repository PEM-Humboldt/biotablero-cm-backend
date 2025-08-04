namespace IAVH.BioTablero.CM.Application.Interfaces.Services;

using IAVH.BioTablero.CM.Application.DTOs.Geo;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;

/// <summary>
/// Location service interface
/// </summary>
public interface ILocationService : IServiceRead<Location, LocationDto, int>
{
}
