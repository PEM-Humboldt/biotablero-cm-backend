namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using System.Linq;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Custom Join Invitation repository interface.
/// </summary>
public interface IJoinInvitationRepository : IRepository<JoinInvitation, int>
{
    /// <summary>
    /// Add initiative filter.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="query">Linq Query.</param>
    /// <returns>Modified Linq query.</returns>
    public IQueryable<JoinInvitation> AddInitiativeFilter(int initiativeId, IQueryable<JoinInvitation> query);

    /// <summary>
    /// Include OData custom entities.
    /// </summary>
    /// <param name="query">Linq Query.</param>
    /// <returns>Modified Linq query.</returns>
    public IQueryable<JoinInvitation> IncludeOdataEntities(IQueryable<JoinInvitation> query);
}
