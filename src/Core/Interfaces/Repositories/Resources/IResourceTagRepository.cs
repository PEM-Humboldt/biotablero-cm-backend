namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.Resources;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;

/// <summary>
/// Resource Tag repository interface.
/// </summary>
public interface IResourceTagRepository : IRepository<ResourceTag, int>
{
    /// <summary>
    /// Check if element is duplicated.
    /// </summary>
    /// <param name="resourceId">Resource identifier.</param>
    /// <param name="tagId">Tag identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    Task<bool> IsDuplicatedAsync(int resourceId, int tagId, CancellationToken ct = default);
}
