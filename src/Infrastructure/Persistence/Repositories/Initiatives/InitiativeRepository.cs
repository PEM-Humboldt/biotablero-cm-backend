namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Initiatives;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

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

    /// <summary>
    /// Finds an entity with the given primary key value.
    /// </summary>
    /// <param name="id">The value of the primary key for the entity to be found.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
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

    /// <summary>
    /// Include OData custom entities.
    /// </summary>
    /// <param name="query">Linq Query.</param>
    /// <returns>Modified Linq query.</returns>
    public IQueryable<Initiative> IncludeOdataEntities(IQueryable<Initiative> query) =>
        query
            .Include(e => e.InitiativeLocations)
                .ThenInclude(e => e.Location)
                    .ThenInclude(e => e.Parent);

    /// <summary>
    /// Get elements by user name.
    /// </summary>
    /// <param name="userName">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Elements list.</returns>
    public async Task<IEnumerable<Initiative>> GetByUserNameAsync(string userName, CancellationToken ct = default) =>
        await dbContext.Initiatives
            .Include(e => e.InitiativeUsers)
            .Where(e => e.InitiativeUsers.Any(e => e.UserName == userName))
            .ToListAsync(ct);

    /// <summary>
    /// Get if elements exists by name.
    /// </summary>
    /// <param name="name">Initiative name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    public async Task<bool> AnyByNameAsync(string name, CancellationToken ct = default) =>
        await dbContext.Initiatives
            .Where(e => e.Name == name)
            .AnyAsync(ct);

    /// <summary>
    /// Get if element is duplicated.
    /// </summary>
    /// <param name="id">Initiative identifier.</param>
    /// <param name="name">Initiative name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    public async Task<bool> IsDuplicatedAsync(int id, string name, CancellationToken ct = default) =>
        await dbContext.Initiatives
            .Where(e => e.Id != id && e.Name == name)
            .AnyAsync(ct);

    /// <summary>
    /// Get if elements exists by tag.
    /// </summary>
    /// <param name="tagId">Tag identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    public async Task<bool> AnyByTagAsync(int tagId, CancellationToken ct = default) =>
        await dbContext.Initiatives
            .Where(e => e.InitiativeTags.Any(e => e.TagId == tagId))
            .AnyAsync(ct);

    /// <summary>
    /// Get polygon centroid.
    /// </summary>
    /// <param name="locationIds">Location identifiers.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Polygon centroid.</returns>
    public async Task<Point> GetCentroidAsync(int[] locationIds, CancellationToken ct = default)
    {
        var geometries = await dbContext.LocationPolygons
            .Where(lp => locationIds.Contains(lp.Id))
            .Select(lp => lp.Geometry)
            .ToListAsync(ct);

        var union = geometries.Aggregate((g1, g2) => g1.Union(g2));

        return union.Centroid;
    }

    /// <summary>
    /// Get count of active initiatives.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Count of active initiatives.</returns>
    public async Task<int> GetActiveInitiativesCountAsync(CancellationToken ct = default) =>
        await dbContext.Initiatives
            .Where(i => i.Enabled)
            .CountAsync(ct);

    /// <summary>
    /// Get total area of active initiatives with area.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Total area in square kilometers.</returns>
    public async Task<double> GetTotalAreaOfActiveInitiativesAsync(CancellationToken ct = default) =>
        await dbContext.Initiatives
            .Where(i => i.Enabled && i.PolygonArea > 0)
            .SumAsync(i => i.PolygonArea, ct);

    /// <summary>
    /// Get count of people involved in active initiatives.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Count of people involved in active initiatives.</returns>
    public async Task<int> GetPeopleInvolvedInActiveInitiativesCountAsync(CancellationToken ct = default) =>
        await dbContext.InitiativeUsers
            .Where(iu => iu.Initiative.Enabled)
            .CountAsync(ct);
}
