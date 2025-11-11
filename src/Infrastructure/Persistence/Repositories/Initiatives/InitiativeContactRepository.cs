namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Initiatives;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Initiative Contact repository.
/// </summary>
public class InitiativeContactRepository : Repository<InitiativeContact, int>, IInitiativeContactRepository
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    public InitiativeContactRepository(GeneralContext dbContext)
        : base(dbContext)
    {
    }

    /// <summary>
    /// Get elements by initiative.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Entities by selected initiative.</returns>
    public async Task<IEnumerable<InitiativeContact>> GetByInitiativeAsync(int initiativeId, CancellationToken ct = default) =>
        await dbContext.InitiativeContacts
            .Where(e => e.InitiativeId == initiativeId)
            .ToListAsync(ct);

    /// <summary>
    /// Check if element is duplicated.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="email">Contact email.</param>
    /// <param name="phone">Contact phone.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    public async Task<bool> IsDuplicatedAsync(int initiativeId, string email, string phone, CancellationToken ct = default) =>
        await dbContext.InitiativeContacts
            .Where(e => e.InitiativeId == initiativeId && (e.Phone == phone || e.Email == email))
            .AnyAsync(ct);

    /// <summary>
    /// Check if element is duplicated.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="email">Contact email.</param>
    /// <param name="phone">Contact phone.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    public async Task<bool> IsDuplicatedAsync(int id, int initiativeId, string email, string phone, CancellationToken ct = default) =>
        await dbContext.InitiativeContacts
            .Where(e => e.Id != id && e.InitiativeId == initiativeId && (e.Phone == phone || e.Email == email))
            .AnyAsync(ct);
}
