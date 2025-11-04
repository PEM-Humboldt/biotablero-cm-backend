namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Initiative User repository interface.
/// </summary>
public interface IInitiativeUserRepository : IRepository<InitiativeUser, int>
{
    /// <summary>
    /// Check if element is duplicated.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    Task<bool> IsDuplicatedAsync(int initiativeId, string userName, CancellationToken ct = default);

    /// <summary>
    /// Get elements by initiative, user and level.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="levelId">Level identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities by selected initiative.</returns>
    Task<bool> AnyByInitiativeUserAndLevelAsync(int initiativeId, string userName, int levelId, CancellationToken ct = default);

    /// <summary>
    /// Get elements by initiative.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities by selected initiative.</returns>
    Task<IEnumerable<InitiativeUser>> GetByInitiativeAsync(int initiativeId, CancellationToken ct = default);

    /// <summary>
    /// Get elements by initiative and level.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="levelId">Level identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities by selected parameters.</returns>
    Task<IEnumerable<InitiativeUser>> GetByInitiativeAndLevelAsync(int initiativeId, int levelId, CancellationToken ct = default);

    /// <summary>
    /// Get elements by initiative and level.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="levelId">Level identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities by selected parameters.</returns>
    Task<IEnumerable<InitiativeUser>> GetByInitiativeAndLevelAsync(int id, int initiativeId, int levelId, CancellationToken ct = default);
}
