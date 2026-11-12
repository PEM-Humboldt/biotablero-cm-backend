namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.Indicators;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

/// <summary>
/// Topic Location repository interface.
/// </summary>
public interface ITopicLocationRepository : IRepository<TopicLocation, int>
{
    /// <summary>
    /// Get elements by topic identifier.
    /// </summary>
    /// <param name="topicId">Topic identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities by selected topic.</returns>
    Task<List<TopicLocation>> GetByTopicAsync(int topicId, CancellationToken ct = default);
}
