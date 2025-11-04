namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Initiatives;

using System.Linq;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Custom Join Invitation repository.
/// </summary>
/// <param name="dbContext">General Database Context.</param>
public class JoinInvitationRepository(GeneralContext dbContext) : Repository<JoinInvitation, int>(dbContext), IJoinInvitationRepository
{
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
