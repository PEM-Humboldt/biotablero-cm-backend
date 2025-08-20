namespace IAVH.BioTablero.CM.Application.Interfaces.Services;

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
public interface IInitiativeService : IServiceRead<Initiative, InitiativeDto, int>, IServiceAdd<InitiativeDto>, IServiceUpdate<InitiativeDto, int>, IServiceDisable<int>
{
    /// <summary>
    /// Upload image.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="formFile">Image data.</param>
    /// <param name="imageType">Initiative image type.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> UploadImage(int id, IInputFile formFile, InitiativeImageType imageType, CancellationToken ct);

    /// <summary>
    /// Get entity polygon.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> GetPolygon(int id, CancellationToken ct = default);

    /// <summary>
    /// Update entity polygon.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="geoJsonString">Polygon data (string).</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> UpdatePolygon(int id, string geoJsonString, CancellationToken ct = default);
}
