namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Tag repository interface.
/// </summary>
public interface ITagRepository : IRepository<Tag, int>
{
    /// <summary>
    /// Get if elements exists by name.
    /// </summary>
    /// <param name="name">Entity name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    public Task<bool> AnyByName(string name, CancellationToken ct = default);

    /// <summary>
    /// Get if element is duplicated.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="name">Entity name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    public Task<bool> IsDuplicated(int id, string name, CancellationToken ct = default);
}
