namespace IAVH.BioTablero.CM.Application.Services.Geo;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Geo;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

/// <summary>
/// Location service.
/// </summary>
public class LocationService : ServiceRead<Location, LocationDto, int>, ILocationService
{
    private new readonly ILocationRepository entityRepository;
    private readonly ILocationPolygonRepository locationPolygonRepository;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="locationPolygonRepository">Location Polygon repository.</param>
    public LocationService(
        ILocationRepository entityRepository,
        IMapper<Location, LocationDto> mapper,
        ILocationPolygonRepository locationPolygonRepository)
        : base(entityRepository, mapper)
    {
        this.entityRepository = entityRepository;
        this.locationPolygonRepository = locationPolygonRepository;
    }

    /// <summary>
    /// Get locations by parent.
    /// </summary>
    /// <param name="parentId">Parent identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> GetByParentAsync(int? parentId, CancellationToken ct = default)
    {
        var dataListEntity = await entityRepository.GetByParentIdAsync(parentId, ct);

        var dataListDto = dataListEntity
            .Select(mapper.Map);

        return new()
        {
            ResponseBody = dataListDto,
        };
    }

    /// <summary>
    /// Get entity polygon (simplified).
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> GetPolygonAsync(int id, CancellationToken ct = default)
    {
        var simplifiedGeometry = await locationPolygonRepository.GetSimplifiedByLocationAsync(id, ct);

        if (!string.IsNullOrEmpty(simplifiedGeometry))
        {
            return new()
            {
                ResponseBody = JsonDocument.Parse(simplifiedGeometry),
            };
        }

        return new(true)
        {
            StatusCode = HttpStatusCode.NotFound,
        };
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
}
