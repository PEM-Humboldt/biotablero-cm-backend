namespace IAVH.BioTablero.CM.Application.Interfaces.Services;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;

/// <summary>
/// Territory Story Image service interface.
/// </summary>
public interface ITerritoryStoryImageService : IRead<TerritoryStoryImage, int>, IAdd<TerritoryStoryImageDto>, IUpdate<TerritoryStoryImageDto, int>, IDelete<int>
{
    /// <summary>
    /// Get entities by initiative.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> GetByInitiativeAsync(int initiativeId, CancellationToken ct = default);
}
