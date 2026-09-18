namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Indicators;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Indicators;

using Microsoft.EntityFrameworkCore;

using Serilog;

/// <summary>
/// Observation repository.
/// </summary>
/// <param name="dbContext">General Database Context.</param>
/// <param name="logger">System logger.</param>
public class ObservationRepository(
    GeneralContext dbContext,
    ILogger logger) : Repository<Observation, int>(dbContext, logger), IObservationRepository
{
    /// <inheritdoc/>
    public override async Task<Observation?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await IncludeCustomEntities()
            .Include(e => e.ObservationLocations!)
                .ThenInclude(e => e.Location)
                    .ThenInclude(e => e!.Parent)
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync(ct);

    /// <inheritdoc/>
    public async Task<IEnumerable<Observation>> GetByInitiativeAsync(int initiativeId, CancellationToken ct = default) =>
        await IncludeCustomEntities()
            .Where(e => e.InitiativeId == initiativeId)
            .ToListAsync(ct);

    /// <inheritdoc/>
    public async Task<IEnumerable<Observation>> GetByInitiativeAndNamesAsync(int initiativeId, string[] names, CancellationToken ct = default) =>
        await dbContext.Observations
            .Where(e => e.InitiativeId == initiativeId && names.Contains(e.Name))
            .ToListAsync(ct);

    /// <inheritdoc/>
    public IQueryable<Observation> IncludeOdataEntities(IQueryable<Observation> query) =>
        IncludeCustomEntities(query)
            .Include(e => e.ObservationLocations!)
                .ThenInclude(e => e.Location)
                    .ThenInclude(e => e!.Parent);

    /// <inheritdoc/>
    public async Task<int> CountAsync(int initiativeId, CancellationToken ct = default) =>
        await dbContext.Observations
            .Include(e => e.Initiative)
            .Where(e => e.Initiative!.Id == initiativeId)
            .CountAsync(ct);

    /// <inheritdoc/>
    public async Task<int[]> GetVersionsAsync(int id, CancellationToken ct = default) =>
        await dbContext.ObservationVersions
            .Where(e => e.ObservationId == id)
            .Select(e => e.Version)
            .ToArrayAsync(ct);

    /// <summary>
    /// Include custom entities.
    /// </summary>
    /// <returns>Modified Linq query.</returns>
    private IQueryable<Observation> IncludeCustomEntities(IQueryable<Observation>? query = null)
    {
        query ??= dbContext.Observations;

        ArgumentNullException.ThrowIfNull(query);

        return query
            .Include(e => e.Topic)
            .Include(e => e.Versions)
            .Include(e => e.ObservationTags!)
                .ThenInclude(e => e.Tag)
            .Include(e => e.Initiative);
    }
}
