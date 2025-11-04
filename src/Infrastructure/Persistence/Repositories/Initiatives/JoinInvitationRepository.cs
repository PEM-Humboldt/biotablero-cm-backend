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

    /// <summary>
    /// Finds an entity with the given primary key value.
    /// </summary>
    /// <param name="id">The value of the primary key for the entity to be found.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public new async Task<JoinInvitation> GetByIdAsync(int id, CancellationToken ct = default) =>
        await dbContext.JoinInvitations
            .Include(e => e.Guests)
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync(ct);

    /// <summary>
    /// Add initiative filter.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="query">Linq Query.</param>
    /// <returns>Modified Linq query.</returns>
    public IQueryable<JoinInvitation> AddInitiativeFilter(int initiativeId, IQueryable<JoinInvitation> query) =>
        query
            .Where(e => e.InitiativeId == initiativeId);

    /// <summary>
    /// Include OData custom entities.
    /// </summary>
    /// <param name="query">Linq Query.</param>
    /// <returns>Modified Linq query.</returns>
    public IQueryable<JoinInvitation> IncludeOdataEntities(IQueryable<JoinInvitation> query) =>
        query
            .Include(e => e.Initiative)
            .Include(e => e.Guests);
}
