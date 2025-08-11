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
}
