namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Initiatives;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;

using Microsoft.EntityFrameworkCore;

using NetTopologySuite.Geometries;

/// <summary>
/// Initiative repository.
/// </summary>
public class InitiativeRepository : Repository<Initiative, int>, IInitiativeRepository
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    public InitiativeRepository(GeneralContext dbContext)
        : base(dbContext)
    {
    }

    /// <inheritdoc/>
    public new async Task<Initiative> GetByIdAsync(int id, CancellationToken ct = default) =>
        await dbContext.Initiatives
            .Include(e => e.InitiativeContacts)
            .Include(e => e.InitiativeUsers)
            .Include(e => e.InitiativeLocations)
                .ThenInclude(e => e.Location)
                    .ThenInclude(e => e.Parent)
            .Include(e => e.InitiativeTags)
                .ThenInclude(e => e.Tag)
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync(ct);

    /// <inheritdoc/>
    public IQueryable<Initiative> IncludeOdataEntities(IQueryable<Initiative> query) =>
        query
            .Include(e => e.InitiativeLocations)
                .ThenInclude(e => e.Location)
                    .ThenInclude(e => e.Parent);

    /// <inheritdoc/>
    public async Task<IEnumerable<Initiative>> GetByUserNameAsync(string userName, CancellationToken ct = default) =>
        await dbContext.Initiatives
            .Include(e => e.InitiativeUsers)
            .Where(e => e.InitiativeUsers.Any(e => e.UserName == userName))
            .ToListAsync(ct);

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
    public async Task<Point> GetCentroidAsync(int[] locationIds, CancellationToken ct = default)
    {
        var geometries = await dbContext.LocationPolygons
            .Where(lp => locationIds.Contains(lp.Id))
            .Select(lp => lp.Geometry)
            .ToListAsync(ct);

        var union = geometries.Aggregate((g1, g2) => g1.Union(g2));

        return union.Centroid;
    }

    /// <inheritdoc/>
    public async Task<int> GetActiveInitiativesCountAsync(CancellationToken ct = default) =>
        await dbContext.Initiatives
            .Where(i => i.Enabled)
            .CountAsync(ct);

    /// <inheritdoc/>
    public async Task<double> GetTotalAreaOfActiveInitiativesAsync(CancellationToken ct = default) =>
        await dbContext.Initiatives
            .Where(i => i.Enabled && i.PolygonArea > 0)
            .SumAsync(i => i.PolygonArea, ct);

    /// <inheritdoc/>
    public async Task<int> GetPeopleInvolvedInActiveInitiativesCountAsync(CancellationToken ct = default) =>
        await dbContext.InitiativeUsers
            .Where(iu => iu.Initiative.Enabled)
            .CountAsync(ct);

    /// <inheritdoc/>
    public async Task<int> GetActiveInitiativesCountByDepartmentAsync(int departmentId, CancellationToken ct = default) =>
        await dbContext.Initiatives
            .Where(i => i.Enabled && i.InitiativeLocations.Any(il => il.LocationId == departmentId))
            .CountAsync(ct);

    /// <inheritdoc/>
    public async Task<double> GetTotalAreaOfActiveInitiativesByDepartmentAsync(int departmentId, CancellationToken ct = default) =>
        await dbContext.Initiatives
            .Where(i => i.Enabled && i.PolygonArea > 0 && i.InitiativeLocations.Any(il => il.LocationId == departmentId))
            .SumAsync(i => i.PolygonArea, ct);

    /// <inheritdoc/>
    public async Task<int> GetPeopleInvolvedInActiveInitiativesCountByDepartmentAsync(int departmentId, CancellationToken ct = default) =>
        await dbContext.InitiativeUsers
            .Where(iu => iu.Initiative.Enabled && iu.Initiative.InitiativeLocations.Any(il => il.LocationId == departmentId))
            .CountAsync(ct);

    /// <inheritdoc/>
    public async Task<int> GetActiveInitiativesCountByInitiativeAsync(int initiativeId, CancellationToken ct = default) =>
        await dbContext.Initiatives
            .Where(i => i.Enabled && i.Id == initiativeId)
            .CountAsync(ct);

    /// <inheritdoc/>
    public async Task<double> GetTotalAreaOfActiveInitiativesByInitiativeAsync(int initiativeId, CancellationToken ct = default) =>
        await dbContext.Initiatives
            .Where(i => i.Enabled && i.PolygonArea > 0 && i.Id == initiativeId)
            .SumAsync(i => i.PolygonArea, ct);

    /// <inheritdoc/>
    public async Task<int> GetPeopleInvolvedInActiveInitiativesCountByInitiativeAsync(int initiativeId, CancellationToken ct = default) =>
        await dbContext.InitiativeUsers
            .Where(iu => iu.Initiative.Enabled && iu.InitiativeId == initiativeId)
            .CountAsync(ct);

    /// <inheritdoc/>
    public async Task<IEnumerable<InitiativeGeoData>> GetActiveInitiativesWithCoordinatesByLocationAsync(int? locationId = null, CancellationToken ct = default)
    {
        var query = dbContext.Initiatives
            .Where(i => i.Enabled && i.Coordinate != null);

        // Filter by location if provided
        if (locationId.HasValue)
        {
            query = query.Where(i => i.InitiativeLocations.Any(il => il.LocationId == locationId.Value));
        }

        var results = await query
            .Select(i => new InitiativeGeoData
            {
                InitiativeId = i.Id,
                InitiativeName = i.Name,
                Coordinate = new double[] { i.Coordinate.X, i.Coordinate.Y },
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
