namespace IAVH.BioTablero.CM.Application.Services.Geo;

using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.DTOs.Geo;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Geo;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Locations;

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
    /// <param name="errorTranslator">Error translator.</param>
    /// <param name="locationPolygonRepository">Location Polygon repository.</param>
    public LocationService(
        ILocationRepository entityRepository,
        IMapperRead<Location, LocationDto> mapper,
        IValidationErrorTranslator errorTranslator,
        ILocationPolygonRepository locationPolygonRepository)
        : base(entityRepository, mapper, errorTranslator)
    {
        this.entityRepository = entityRepository;
        this.locationPolygonRepository = locationPolygonRepository;
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetByParentAsync(int? parentId, CancellationToken ct = default)
    {
        if (!parentId.HasValue)
        {
            parentId = GeoConstants.DefaultNationId;
        }

        var dataListEntity = await entityRepository.GetByParentIdAsync(parentId.Value, ct);

        var dataListDto = dataListEntity
            .Select(mapper.Map);

        return new()
        {
            ResponseBody = dataListDto,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetPolygonAsync(int id, CancellationToken ct = default)
    {
        var simplifiedGeometry = await locationPolygonRepository.GetSimplifiedByLocationAsync(id, ct);
        using var doc = JsonDocument.Parse(simplifiedGeometry);

        if (!string.IsNullOrEmpty(simplifiedGeometry))
        {
            return new()
            {
                ResponseBody = doc.RootElement.Clone(),
            };
        }

        return new(true)
        {
            StatusCode = HttpStatusCode.NotFound,
        };
    }
}
