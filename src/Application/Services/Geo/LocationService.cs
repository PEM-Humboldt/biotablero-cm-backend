namespace IAVH.BioTablero.CM.Application.Services.Geo;

using System.Linq;
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
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    public LocationService(
        IRepository<Location> entityRepository,
        IMapper<Location, LocationDto> mapper)
        : base(entityRepository, mapper)
    {
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
}
