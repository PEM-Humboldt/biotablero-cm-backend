namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.Indicators;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;

/// <summary>
/// Central Actor repository interface.
/// </summary>
public interface ICentralActorRepository : IRepository<CentralActor, int>
{
    /// <summary>
    /// Get elements by names.
    /// </summary>
    /// <param name="names">Names list.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Selected entities.</returns>
    Task<IEnumerable<CentralActor>> GetByNamesAsync(string[] names, CancellationToken ct = default);
}
