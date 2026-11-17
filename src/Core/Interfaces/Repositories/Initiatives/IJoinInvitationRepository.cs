namespace IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Join Invitation repository interface.
/// </summary>
public interface IJoinInvitationRepository : IRepository<JoinInvitation, int>
{
    /// <summary>
    /// Get elements by initiative and users.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="userNames">User names.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    Task<bool> AnyAsync(int initiativeId, string[] userNames, CancellationToken ct = default);

    /// <summary>
    /// Add initiative filter.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="query">Linq Query.</param>
    /// <returns>Modified Linq query.</returns>
    IQueryable<JoinInvitation> AddInitiativeFilter(int initiativeId, IQueryable<JoinInvitation> query);

    /// <summary>
    /// Include OData custom entities.
    /// </summary>
    /// <param name="query">Linq Query.</param>
    /// <returns>Modified Linq query.</returns>
    IQueryable<JoinInvitation> IncludeOdataEntities(IQueryable<JoinInvitation> query);
}
