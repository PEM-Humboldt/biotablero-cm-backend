namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Initiatives;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Initiative User repository.
/// </summary>
public class InitiativeUserRepository : Repository<InitiativeUser, int>, IInitiativeUserRepository
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    public InitiativeUserRepository(GeneralContext dbContext)
        : base(dbContext)
    {
    }

    /// <summary>
    /// Check if element is duplicated.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    public async Task<bool> IsDuplicatedAsync(int initiativeId, string userName, CancellationToken ct = default) =>
        await dbContext.InitiativeUsers
            .Where(e => e.InitiativeId == initiativeId && e.UserName == userName)
            .AnyAsync(ct);

    /// <summary>
    /// Get elements by initiative, user and level.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="userName">User name.</param>
    /// <param name="levelId">Level identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities by selected initiative.</returns>
    public async Task<bool> AnyByInitiativeUserAndLevelAsync(int initiativeId, string userName, int levelId, CancellationToken ct = default) =>
        await dbContext.InitiativeUsers
            .Where(e => e.InitiativeId == initiativeId && e.UserName == userName && e.LevelId == levelId)
            .AnyAsync(ct);

    /// <summary>
    /// Get elements by initiative.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities by selected initiative.</returns>
    public async Task<IEnumerable<InitiativeUser>> GetByInitiativeAsync(int initiativeId, CancellationToken ct = default) =>
        await dbContext.InitiativeUsers
            .Where(e => e.InitiativeId == initiativeId)
            .ToListAsync(ct);

    /// <summary>
    /// Get elements by initiative and level.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="levelId">Level identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities by selected parameters.</returns>
    public async Task<IEnumerable<InitiativeUser>> GetByInitiativeAndLevelAsync(int initiativeId, int levelId, CancellationToken ct = default) =>
        await dbContext.InitiativeUsers
            .Where(e => e.InitiativeId == initiativeId && e.LevelId == levelId)
            .ToListAsync(ct);

    /// <summary>
    /// Get elements by initiative and level.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="levelId">Level identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities by selected parameters.</returns>
    public async Task<IEnumerable<InitiativeUser>> GetByInitiativeAndLevelAsync(int id, int initiativeId, int levelId, CancellationToken ct = default) =>
        await dbContext.InitiativeUsers
            .Where(e => e.InitiativeId == initiativeId && e.LevelId == levelId)
            .ToListAsync(ct);
}
