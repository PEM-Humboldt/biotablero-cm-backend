namespace IAVH.BioTablero.CM.Application.Interfaces.Services;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Geo;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Geo;

/// <summary>
/// Location service interface.
/// </summary>
public interface ILocationService : IRead<Location, LocationDto, int>
{
    /// <summary>
    /// Get locations by parent.
    /// </summary>
    /// <param name="parentId">Parent identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> GetByParentAsync(int? parentId, CancellationToken ct = default);

    /// <summary>
    /// Get entity polygon (simplified).
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> GetPolygonAsync(int id, CancellationToken ct = default);
}
