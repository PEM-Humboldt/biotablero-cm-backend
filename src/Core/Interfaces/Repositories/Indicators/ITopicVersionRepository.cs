namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.Indicators;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

/// <summary>
/// Topic Version repository interface.
/// </summary>
public interface ITopicVersionRepository : IRepository<TopicVersion, int>
{
    /// <summary>
    /// Get last version number.
    /// </summary>
    /// <param name="topicId">Topic identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The last version for the specified topic.</returns>
    Task<int> GetLastVersionAsync(int topicId, CancellationToken ct = default);
}
