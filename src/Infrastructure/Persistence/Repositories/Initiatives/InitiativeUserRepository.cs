namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Initiatives;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;

using Microsoft.EntityFrameworkCore;

using Serilog;

/// <summary>
/// Initiative User repository.
/// </summary>
public class InitiativeUserRepository : Repository<InitiativeUser, int>, IInitiativeUserRepository
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    /// <param name="logger">System logger.</param>
    public InitiativeUserRepository(
        GeneralContext dbContext,
        ILogger logger)
        : base(dbContext, logger)
    {
    }

    /// <inheritdoc/>
    public async Task<bool> IsDuplicatedAsync(int initiativeId, string userName, CancellationToken ct = default) =>
        await dbContext.InitiativeUsers
            .Where(e => e.InitiativeId == initiativeId && e.UserName == userName)
            .AnyAsync(ct);

    /// <inheritdoc/>
    public async Task<bool> AnyByInitiativeUserAndLevelAsync(int initiativeId, string userName, int? levelId, CancellationToken ct = default) =>
        await dbContext.InitiativeUsers
            .Where(e =>
                e.InitiativeId == initiativeId &&
                e.UserName == userName &&
                (!levelId.HasValue || e.LevelId == levelId))
            .AnyAsync(ct);

    /// <inheritdoc/>
    public async Task<bool> AnyByInitiativeUserAndLevelsAsync(int initiativeId, string userName, int[] authorizedLevels, CancellationToken ct = default) =>
        await dbContext.InitiativeUsers
            .Where(e =>
                e.InitiativeId == initiativeId &&
                e.UserName == userName &&
                authorizedLevels.Contains(e.LevelId))
            .AnyAsync(ct);

    /// <inheritdoc/>
    public async Task<bool> AnyByInitiativeAndUsersAsync(int initiativeId, string[] userNames, CancellationToken ct = default) =>
        await dbContext.InitiativeUsers
            .Where(e => e.InitiativeId == initiativeId && userNames.Contains(e.UserName))
            .AnyAsync(ct);

    /// <inheritdoc/>
    public async Task<InitiativeUser> GetByInitiativeAndUserNameAsync(int initiativeId, string userName, CancellationToken ct = default) =>
        await dbContext.InitiativeUsers
            .Where(e => e.InitiativeId == initiativeId && e.UserName == userName)
            .FirstOrDefaultAsync(ct);

    /// <inheritdoc/>
    public async Task<IEnumerable<InitiativeUser>> GetByInitiativeAsync(int initiativeId, CancellationToken ct = default) =>
        await dbContext.InitiativeUsers
            .Where(e => e.InitiativeId == initiativeId)
            .ToListAsync(ct);

    /// <inheritdoc/>
    public async Task<IEnumerable<InitiativeUser>> GetByInitiativeAndLevelAsync(int initiativeId, int levelId, CancellationToken ct = default) =>
        await dbContext.InitiativeUsers
            .Where(e => e.InitiativeId == initiativeId && e.LevelId == levelId)
            .ToListAsync(ct);

    /// <inheritdoc/>
    public async Task<IEnumerable<InitiativeUser>> GetByInitiativeAndLevelAsync(int id, int initiativeId, int levelId, CancellationToken ct = default) =>
        await dbContext.InitiativeUsers
            .Where(e => e.InitiativeId == initiativeId && e.LevelId == levelId)
            .ToListAsync(ct);
}
