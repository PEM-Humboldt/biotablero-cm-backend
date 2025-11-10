namespace IAVH.BioTablero.CM.Application.Services.TerritoryStory;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.TerritoryStories;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.TerritoryStories;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

/// <summary>
/// Territory Story Video service.
/// </summary>
public class TerritoryStoryVideoService : ServiceRead<TerritoryStoryVideo, TerritoryStoryVideoDto, int>, ITerritoryStoryVideoService
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    public TerritoryStoryVideoService(
        IRepository<TerritoryStoryVideo, int> entityRepository,
        IMapper<TerritoryStoryVideo,
        TerritoryStoryVideoDto> mapper)
        : base(entityRepository, mapper)
    {
    }

    /// <summary>
    /// Add element.
    /// </summary>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public Task<CustomWebResponse> AddAsync(TerritoryStoryVideoDto entityData, CancellationToken ct = default) => throw new System.NotImplementedException();

    /// <summary>
    /// Update element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public Task<CustomWebResponse> UpdateAsync(int id, TerritoryStoryVideoDto entityData, CancellationToken ct = default) => throw new System.NotImplementedException();

    /// <summary>
    /// Delete element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public Task<CustomWebResponse> DeleteAsync(int id, CancellationToken ct = default) => throw new System.NotImplementedException();
}
