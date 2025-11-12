namespace IAVH.BioTablero.CM.Application.Interfaces.Services.TerritoryStory;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;

/// <summary>
/// Territory Story service interface.
/// </summary>
public interface ITerritoryStoryService : IRead<TerritoryStory, int>, IAdd<TerritoryStoryDto>, IUpdate<TerritoryStoryDto, int>, IDisable<int>
{
    /// <summary>
    /// Get entities by initiative.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> GetByInitiativeAsync(int initiativeId, CancellationToken ct = default);

    /// <summary>
    /// Like button action.
    /// </summary>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> LikeActionAsync(TerritoryStoryLikeDto entityData, CancellationToken ct);
}
