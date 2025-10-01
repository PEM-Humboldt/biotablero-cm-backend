namespace IAVH.BioTablero.CM.Application.Services.Geo;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using NetTopologySuite.Geometries;

using LocationEntity = IAVH.BioTablero.CM.Core.Domain.Entities.Geo.Location;

/// <summary>
/// Service for calculating location areas.
/// </summary>
public class LocationAreaService : ILocationAreaService
{
    private readonly IRepository<LocationEntity> locationRepository;
    private readonly IRepository<LocationPolygon> locationPolygonRepository;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="locationRepository">Location repository.</param>
    /// <param name="locationPolygonRepository">Location polygon repository.</param>
    public LocationAreaService(
        IRepository<LocationEntity> locationRepository,
        IRepository<LocationPolygon> locationPolygonRepository)
    {
        this.locationRepository = locationRepository;
        this.locationPolygonRepository = locationPolygonRepository;
    }

    /// <summary>
    /// Calculate total area for a collection of location IDs.
    /// </summary>
    /// <param name="locationIds">Collection of location IDs.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Total area in square kilometers.</returns>
    public async Task<double> CalculateTotalAreaForLocationsAsync(IEnumerable<int> locationIds, CancellationToken ct = default)
    {
        if (locationIds == null || !locationIds.Any())
        {
            return 0;
        }

        var locationIdsList = locationIds.ToList();
        var totalArea = 0.0;

        // Get all location polygons for the specified locations
        var locationPolygonsQuery = locationPolygonRepository.GetQueryable()
            .Where(lp => locationIdsList.Contains(lp.LocationId) && lp.Geometry != null);

        var locationPolygons = await locationPolygonRepository.QueryToListAsync(locationPolygonsQuery, ct);

        // Calculate area for each polygon
        totalArea = locationPolygons
            .Where(lp => lp.Geometry != null && !lp.Geometry.IsEmpty)
            .Select(lp => GeometryUtils.CalculateAreaInSquareKilometers(lp.Geometry))
            .Sum();

        return Math.Round(totalArea, 6);
    }

    /// <summary>
    /// Calculate area for a single location.
    /// </summary>
    /// <param name="locationId">Location ID.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Area in square kilometers.</returns>
    public async Task<double> CalculateAreaForLocationAsync(int locationId, CancellationToken ct = default)
    {
        var locationPolygonQuery = locationPolygonRepository.GetQueryable()
            .Where(lp => lp.LocationId == locationId && lp.Geometry != null);

        var locationPolygons = await locationPolygonRepository.QueryToListAsync(locationPolygonQuery, ct);
        var locationPolygon = locationPolygons.FirstOrDefault();

        if (locationPolygon?.Geometry == null || locationPolygon.Geometry.IsEmpty)
        {
            return 0;
        }

        return GeometryUtils.CalculateAreaInSquareKilometers(locationPolygon.Geometry);
    }

    /// <summary>
    /// Calculate area for locations by their codes.
    /// </summary>
    /// <param name="locationCodes">Collection of location codes.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Total area in square kilometers.</returns>
    public async Task<double> CalculateTotalAreaForLocationCodesAsync(IEnumerable<string> locationCodes, CancellationToken ct = default)
    {
        if (locationCodes == null || !locationCodes.Any())
        {
            return 0;
        }

        var locationCodesList = locationCodes.ToList();

        // Get location IDs by codes
        var locationsQuery = locationRepository.GetQueryable()
            .Where(l => locationCodesList.Contains(l.Code));

        var locations = await locationRepository.QueryToListAsync(locationsQuery, ct);
        var locationIds = locations.Select(l => l.Id);

        return await CalculateTotalAreaForLocationsAsync(locationIds, ct);
    }
}
