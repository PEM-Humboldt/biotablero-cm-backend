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
/// Territory Story service.
/// </summary>
public class TerritoryStoryService : ServiceRead<TerritoryStory, TerritoryStoryDto, int>, ITerritoryStoryService
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    public TerritoryStoryService(
        IRepository<TerritoryStory, int> entityRepository,
        IMapper<TerritoryStory,
        TerritoryStoryDto> mapper)
        : base(entityRepository, mapper)
    {
    }

    /// <summary>
    /// Add element.
    /// </summary>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public Task<CustomWebResponse> AddAsync(TerritoryStoryDto entityData, CancellationToken ct = default) => throw new System.NotImplementedException();

    /// <summary>
    /// Update element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public Task<CustomWebResponse> UpdateAsync(int id, TerritoryStoryDto entityData, CancellationToken ct = default) => throw new System.NotImplementedException();

    /// <summary>
    /// Enable element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public Task<CustomWebResponse> EnableAsync(int id, CancellationToken ct = default) => throw new System.NotImplementedException();

    /// <summary>
    /// Disable element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public Task<CustomWebResponse> DisableAsync(int id, CancellationToken ct = default) => throw new System.NotImplementedException();
}
