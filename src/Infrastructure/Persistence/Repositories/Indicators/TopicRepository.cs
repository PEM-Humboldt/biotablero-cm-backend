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
/// Topic repository.
/// </summary>
/// <param name="dbContext">General Database Context.</param>
/// <param name="logger">System logger.</param>
public class TopicRepository(
    GeneralContext dbContext,
    ILogger logger) : Repository<Topic, int>(dbContext, logger), ITopicRepository
{
    /// <inheritdoc/>
    public override async Task<Topic?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await IncludeCustomEntities()
            .Include(e => e.TopicLocations!)
                .ThenInclude(e => e.Location)
                    .ThenInclude(e => e!.Parent)
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync(ct);

    /// <inheritdoc/>
    public async Task<IEnumerable<Topic>> GetByInitiativeAsync(int initiativeId, CancellationToken ct = default) =>
        await IncludeCustomEntities()
            .Where(e => e.InitiativeId == initiativeId)
            .ToListAsync(ct);

    /// <inheritdoc/>
    public async Task<IEnumerable<Topic>> GetByInitiativeAndNamesAsync(int initiativeId, string[] names, CancellationToken ct = default) =>
        await dbContext.Topics
            .Include(e => e.Versions)
            .Where(e => e.InitiativeId == initiativeId && names.Contains(e.Name))
            .ToListAsync(ct);

    /// <inheritdoc/>
    public IQueryable<Topic> IncludeOdataEntities(IQueryable<Topic> query) =>
        IncludeCustomEntities(query)
            .Include(e => e.TopicLocations!)
                .ThenInclude(e => e.Location)
                    .ThenInclude(e => e!.Parent);

    /// <inheritdoc/>
    public async Task<int> CountAsync(int initiativeId, CancellationToken ct = default) =>
        await dbContext.Topics
            .Include(e => e.Initiative)
            .Where(e => e.Initiative!.Id == initiativeId)
            .CountAsync(ct);

    /// <inheritdoc/>
    public async Task<bool> IsDuplicatedAsync(int id, int initiativeId, string name, CancellationToken ct = default) =>
        await dbContext.Topics
            .Where(e => e.Id != id && e.InitiativeId == initiativeId && e.Name == name)
            .AnyAsync(ct);

    /// <inheritdoc/>
    public async Task<int[]> GetVersionsAsync(int id, CancellationToken ct = default) =>
        await dbContext.TopicVersions
            .Where(e => e.TopicId == id)
            .Select(e => e.Version)
            .ToArrayAsync(ct);

    /// <summary>
    /// Include custom entities.
    /// </summary>
    /// <returns>Modified Linq query.</returns>
    private IQueryable<Topic> IncludeCustomEntities(IQueryable<Topic>? query = null)
    {
        query ??= dbContext.Topics;

        ArgumentNullException.ThrowIfNull(query);

        return query
            .Include(e => e.TopicType)
            .Include(e => e.Versions)
            .Include(e => e.TopicTags!)
                .ThenInclude(e => e.Tag)
            .Include(e => e.Initiative);
    }
}
