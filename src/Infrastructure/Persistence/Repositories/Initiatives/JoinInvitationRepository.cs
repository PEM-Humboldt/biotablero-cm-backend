namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Initiatives;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Join Invitation repository.
/// </summary>
public class JoinInvitationRepository : Repository<JoinInvitation, int>, IJoinInvitationRepository
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    public JoinInvitationRepository(GeneralContext dbContext)
        : base(dbContext)
    {
    }

    /// <inheritdoc/>
    public new async Task<JoinInvitation> GetByIdAsync(int id, CancellationToken ct = default) =>
        await dbContext.JoinInvitations
            .Include(e => e.Guests)
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync(ct);

    /// <inheritdoc/>
    public IQueryable<JoinInvitation> AddInitiativeFilter(int initiativeId, IQueryable<JoinInvitation> query) =>
        query
            .Where(e => e.InitiativeId == initiativeId);

    /// <inheritdoc/>
    public IQueryable<JoinInvitation> IncludeOdataEntities(IQueryable<JoinInvitation> query) =>
        query
            .Include(e => e.Initiative)
            .Include(e => e.Guests);
}
