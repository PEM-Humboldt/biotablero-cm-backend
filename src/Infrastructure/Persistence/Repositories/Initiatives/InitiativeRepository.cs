namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Initiatives;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Models.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;

using Microsoft.EntityFrameworkCore;

using Serilog;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.GeoEnums;

using InitiativeUserLevelEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.InitiativeUserLevel;

/// <summary>
/// Initiative repository.
/// </summary>
public class InitiativeRepository : Repository<Initiative, int>, IInitiativeRepository
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    /// <param name="logger">System logger.</param>
    public InitiativeRepository(
        GeneralContext dbContext,
        ILogger logger)
        : base(dbContext, logger)
    {
    }

    /// <inheritdoc/>
    public async Task<Initiative> GetByIdAsync(int id, bool userIsAuthenticated, CancellationToken ct = default)
    {
        IQueryable<Initiative> query = dbContext.Initiatives
                    .Include(e => e.InitiativeUsers)
                    .Include(e => e.InitiativeLocations)
                        .ThenInclude(e => e.Location)
                            .ThenInclude(e => e.Parent)
                    .Include(e => e.InitiativeTags)
                        .ThenInclude(e => e.Tag);

        if (userIsAuthenticated)
        {
            query = query.Include(e => e.InitiativeContacts);
        }

        return await query
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync(ct);
    }

    /// <inheritdoc/>
    public IQueryable<Initiative> IncludeOdataEntities(IQueryable<Initiative> query) =>
        query
            .Include(e => e.InitiativeLocations)
                .ThenInclude(e => e.Location)
                    .ThenInclude(e => e.Parent);

    /// <inheritdoc/>
    public async Task<IEnumerable<Initiative>> GetByUserNameAsync(string userName, CancellationToken ct = default) =>
        await dbContext.Initiatives
            .Include(e => e.InitiativeUsers.Where(u => u.UserName == userName))
            .Where(e => e.InitiativeUsers.Any(e => e.UserName == userName))
            .ToListAsync(ct);

    /// <inheritdoc/>
    public async Task<bool> AuthorizedEntityModifyAsync(int id, string userName, bool userIsAdmin, CancellationToken ct = default)
    {
        if (userIsAdmin)
        {
            return true;
        }

        return await dbContext.Initiatives
            .Include(e => e.InitiativeUsers)
            .Where(e =>
                e.Id == id &&
                e.InitiativeUsers.Any(e => e.UserName == userName && e.LevelId == (int)InitiativeUserLevelEnum.Leader))
            .AnyAsync(ct);
    }

    /// <inheritdoc/>
    public async Task<bool> AnyByNameAsync(string name, CancellationToken ct = default) =>
        await dbContext.Initiatives
            .Where(e => e.Name == name)
            .AnyAsync(ct);

    /// <inheritdoc/>
    public async Task<bool> IsDuplicatedAsync(int id, string name, CancellationToken ct = default) =>
        await dbContext.Initiatives
            .Where(e => e.Id != id && e.Name == name)
            .AnyAsync(ct);

    /// <inheritdoc/>
    public async Task<bool> AnyByTagAsync(int tagId, CancellationToken ct = default) =>
        await dbContext.Initiatives
            .Where(e => e.InitiativeTags.Any(e => e.TagId == tagId))
            .AnyAsync(ct);

    /// <inheritdoc/>
    public async Task<IEnumerable<InitiativeGeoData>> GetActiveInitiativesWithCoordinatesByLocationAsync(int? locationId = null, CancellationToken ct = default)
    {
        var query = dbContext.Initiatives
            .Where(i => i.Enabled && i.Coordinate != null);

        // Discard filter for default nation identifier
        if (locationId.HasValue && locationId == GeoConstants.DefaultNationId)
        {
            locationId = null;
        }

        // Filter by location if provided
        if (locationId.HasValue)
        {
            query = query.Where(i => i.InitiativeLocations.Any(il =>
                il.LocationId == locationId.Value &&
                (il.Location.Level == (byte)LocationLevel.Department || il.Location.Level == (byte)LocationLevel.Municipality)));
        }

        var results = await query
            .Select(i => new InitiativeGeoData
            {
                InitiativeId = i.Id,
                InitiativeName = i.Name,
                InitiativeShortName = i.ShortName,
                Coordinate = new double[] { i.Coordinate.X, i.Coordinate.Y },
                MainLocationId = i.MainLocationId,
            })
            .ToListAsync(ct);

        return results;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Initiative>> GetLastEntitiesAsync(int count, CancellationToken ct = default) =>
        await dbContext.Initiatives
            .Where(e => e.Enabled)
            .OrderByDescending(e => e.CreationDate)
            .Take(count)
            .ToListAsync(ct);
}
