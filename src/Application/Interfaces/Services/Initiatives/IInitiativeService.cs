namespace IAVH.BioTablero.CM.Application.Interfaces.Services.Initiatives;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.ExternalServices;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;

/// <summary>
/// Initiative service interface.
/// </summary>
public interface IInitiativeService : IRead<Initiative, int>, IAdd<InitiativeDto>, IUpdate<InitiativeDto, int>, IDisable<int>
{
    /// <summary>
    /// Get entities by user name.
    /// </summary>
    /// <param name="userName">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> GetByUserNameAsync(string userName, CancellationToken ct = default);

    /// <summary>
    /// Upload image.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="formFile">Image data.</param>
    /// <param name="imageType">Initiative image type.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> UploadImageAsync(int id, IInputFile formFile, InitiativeImageType imageType, CancellationToken ct = default);

    /// <summary>
    /// Get entity polygon.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> GetPolygonAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Update entity polygon.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="geoJsonString">Polygon data (string).</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> UpdatePolygonAsync(int id, string geoJsonString, CancellationToken ct = default);

    /// <summary>
    /// Get active initiatives with coordinates by location.
    /// </summary>
    /// <param name="locationId">Location identifier (optional). If null, returns all active initiatives.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> GetByLocationAsync(int? locationId = null, CancellationToken ct = default);

    /// <summary>
    /// Get last created entities.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> GetLastEntitiesAsync(CancellationToken ct = default);
}
