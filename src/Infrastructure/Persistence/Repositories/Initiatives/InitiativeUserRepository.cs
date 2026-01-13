namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Initiatives;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;

using Microsoft.EntityFrameworkCore;

using InitiativeUserLevelEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.InitiativeUserLevel;

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

    /// <inheritdoc/>
    public async Task<bool> IsDuplicatedAsync(int initiativeId, string userName, CancellationToken ct = default) =>
        await dbContext.InitiativeUsers
            .Where(e => e.InitiativeId == initiativeId && e.UserName == userName)
            .AnyAsync(ct);

    /// <inheritdoc/>
    public async Task<bool> AnyByInitiativeUserAndLevelAsync(int initiativeId, string userName, int levelId, CancellationToken ct = default) =>
        await dbContext.InitiativeUsers
            .Where(e => e.InitiativeId == initiativeId && e.UserName == userName && e.LevelId == levelId)
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

    /// <inheritdoc/>
    public async Task<bool> AuthorizedEntityModifyAsync(string userName, CancellationToken ct = default) =>
        await dbContext.InitiativeUsers
            .Where(e => e.UserName == userName && e.LevelId == (int)InitiativeUserLevelEnum.Leader)
            .AnyAsync(ct);
}
