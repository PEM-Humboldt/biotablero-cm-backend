namespace IAVH.BioTablero.CM.Application.Interfaces.Services;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;

/// <summary>
/// Territory Story Video service interface.
/// </summary>
public interface ITerritoryStoryVideoService : IRead<TerritoryStoryVideo, int>, IAdd<TerritoryStoryVideoDto>, IUpdate<TerritoryStoryVideoDto, int>, IDelete<int>
{
    /// <summary>
    /// Get elements by territory story.
    /// </summary>
    /// <param name="territoryStoryId">Territory Story identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities by selected territory story.</returns>
    Task<CustomWebResponse> GetByTerritoryStoryAsync(int territoryStoryId, CancellationToken ct = default);
}
