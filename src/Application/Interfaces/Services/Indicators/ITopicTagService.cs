namespace IAVH.BioTablero.CM.Application.Interfaces.Services.Indicators;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Topic Tag service interface.
/// </summary>
public interface ITopicTagService : IDelete<int>
{
    /// <summary>
    /// Add element.
    /// </summary>
    /// <param name="topicId">Topic identifier.</param>
    /// <param name="tagId">Tag identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> AddAsync(int topicId, int tagId, CancellationToken ct = default);
}
