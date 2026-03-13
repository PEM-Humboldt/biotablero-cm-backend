namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Initiatives;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;

using Microsoft.EntityFrameworkCore;

using Serilog;

/// <summary>
/// Initiative Tag repository.
/// </summary>
public class InitiativeTagRepository : Repository<InitiativeTag, int>, IInitiativeTagRepository
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    /// <param name="logger">System logger.</param>
    public InitiativeTagRepository(
        GeneralContext dbContext,
        ILogger logger)
        : base(dbContext, logger)
    {
    }

    /// <inheritdoc/>
    public override async Task<InitiativeTag> GetByIdAsync(int id, CancellationToken ct = default) =>
        await dbContext.InitiativeTags
                .Include(e => e.Tag)
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync(ct);

    /// <inheritdoc/>
    public override async Task<InitiativeTag> AddAsync(InitiativeTag entity, CancellationToken ct = default)
    {
        await base.AddAsync(entity, ct);
        return await GetByIdAsync(entity.Id, ct);
    }

    /// <inheritdoc/>
    public async Task<bool> IsDuplicatedAsync(int initiativeId, int tagId, CancellationToken ct = default) =>
        await dbContext.InitiativeTags
            .Where(e => e.InitiativeId == initiativeId && e.TagId == tagId)
            .AnyAsync(ct);
}
