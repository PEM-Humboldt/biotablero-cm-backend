namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Initiatives;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;


using Microsoft.EntityFrameworkCore;

/// <summary>
/// Initiative Tag repository.
/// </summary>
public class InitiativeTagRepository : Repository<InitiativeTag, int>, IInitiativeTagRepository
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    public InitiativeTagRepository(GeneralContext dbContext)
        : base(dbContext)
    {
    }

    /// <summary>
    /// Check if element is duplicated.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="tagId">Tag identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    public async Task<bool> IsDuplicatedAsync(int initiativeId, int tagId, CancellationToken ct = default) =>
        await dbContext.InitiativeTags
            .Where(e => e.InitiativeId == initiativeId && e.TagId == tagId)
            .AnyAsync(ct);
}
