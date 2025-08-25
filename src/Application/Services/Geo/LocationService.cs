namespace IAVH.BioTablero.CM.Application.Services.Geo;

using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Geo;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Specifications;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

/// <summary>
/// Location service.
/// </summary>
public class LocationService : ServiceRead<Location, LocationDto, int, LocationSpec>, ILocationService
{
    private readonly IRepository<LocationPolygon> locationPolygonRepository;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="locationPolygonRepository">Location Polygon repository.</param>
    public LocationService(
        IRepository<Location> entityRepository,
        IMapper<Location, LocationDto> mapper,
        IRepository<LocationPolygon> locationPolygonRepository)
        : base(entityRepository, mapper)
    {
        this.locationPolygonRepository = locationPolygonRepository;
    }

    /// <summary>
    /// Get locations by parent.
    /// </summary>
    /// <param name="parentId">Parent identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> GetByParent(int? parentId, CancellationToken ct = default)
    {
        var dataListEntity = await entityRepository.ListAsync(LocationSpec.ParentIdSpec(parentId), ct);

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
    public async Task<CustomWebResponse> GetPolygon(int id, CancellationToken ct = default)
    {
        var entity = await locationPolygonRepository.FirstOrDefaultAsync(LocationPolygonSpec.LocationIdSpec(id), ct);

        if (entity != null)
        {
            return new()
            {
                ResponseBody = JsonDocument.Parse(entity.GeometrySimplified),
            };
        }

        return new(true)
        {
            StatusCode = HttpStatusCode.NotFound,
        };
    }
}
