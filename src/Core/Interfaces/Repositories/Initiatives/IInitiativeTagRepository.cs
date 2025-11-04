namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Initiative Tag repository interface.
/// </summary>
public interface IInitiativeTagRepository : IRepository<InitiativeTag, int>
{
    /// <summary>
    /// Check if element is duplicated.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="tagId">Tag identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    public Task<bool> IsDuplicatedAsync(int initiativeId, int tagId, CancellationToken ct = default);
}
