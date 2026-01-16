namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Resources;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Resources;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Resource repository.
/// </summary>
public class ResourceRepository : Repository<Resource, int>, IResourceRepository
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    public ResourceRepository(GeneralContext dbContext)
        : base(dbContext)
    {
    }

    /// <inheritdoc/>
    public async Task<bool> AuthorizedEntityModifyAsync(int id, string userName, CancellationToken ct = default) =>
        await dbContext.Resources
            .Include(e => e.Initiative)
                .ThenInclude(e => e.InitiativeUsers)
            .Where(e => e.Id == id && e.Initiative.InitiativeUsers.Any(e => e.UserName == userName))
            .AnyAsync(ct);
}
