namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Initiatives;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Models.Initiatives;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;

using Microsoft.EntityFrameworkCore;

using NetTopologySuite.Geometries;

using Npgsql;

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
    public async Task<Point> GetCentroidAsync(int[] locationIds, CancellationToken ct = default) =>
        await dbContext
            .Database
            .SqlQuery<Point>(
                @$"
                    SELECT ST_Centroid(ST_Union(lp.geometry)) AS ""Value""
                    FROM ""geo"".""location_polygon"" lp
                    WHERE lp.location_id = ANY ({locationIds})
                ")
            .FirstOrDefaultAsync(ct);

    /// <inheritdoc/>
    public async Task<Point> GetCentroidAsync(int initiativeId, CancellationToken ct = default) =>
        await dbContext
            .Database
            .SqlQuery<Point>(
                @$"
                    SELECT ST_Centroid(ST_Union(lp.geometry)) AS ""Value""
                    FROM ""initiatives"".""initiative_location"" il
                    INNER JOIN ""geo"".""location_polygon"" lp ON il.location_id = lp.location_id 
                    WHERE il.initiative_id = {initiativeId}
                ")
            .FirstOrDefaultAsync(ct);

    /// <inheritdoc/>
    public async Task<int> GetEnabledRecordsCountAsync(string userName = null, int? departmentId = null, int? initiativeId = null, CancellationToken ct = default) =>
        await dbContext.Initiatives
            .Include(e => e.InitiativeUsers)
            .Include(e => e.InitiativeLocations)
                .ThenInclude(e => e.Location)
            .Where(e => e.Enabled &&
                (userName == null || e.InitiativeUsers.Any(e => e.UserName == userName)) &&
                    (departmentId == null ||
                        e.MainLocationId == departmentId ||
                        e.InitiativeLocations.Any(e => e.LocationId == departmentId &&
                            e.Location.Level == (byte)LocationLevel.Department)) &&
                    (initiativeId == null || e.Id == initiativeId))
            .Distinct()
            .CountAsync(ct);

    /// <inheritdoc/>
    public async Task<double> GetAreaAsync(int? departmentId = null, int? initiativeId = null, CancellationToken ct = default)
    {
        var sql = """
            SELECT
                COALESCE(
                    ST_Area(
                        ST_Transform(
                            ST_UnaryUnion(geom),
                            3116
                        )
                    ),
                    0
                ) AS "Value"
            FROM (
                SELECT DISTINCT lp.geometry AS geom
                FROM initiatives.initiative_location il
                INNER JOIN initiatives.initiative i
                    ON i.id = il.initiative_id
                INNER JOIN geo.location l
                    ON l.id = il.location_id
                INNER JOIN geo.location_polygon lp
                    ON lp.location_id = l.id
                WHERE
                    i.enabled = TRUE
                    AND i.polygon_area > 0

                    AND (
                        @departmentId::int IS NULL
                        OR i.main_location_id = @departmentId::int

                        OR EXISTS (
                            SELECT 1
                            FROM initiatives.initiative_location il2
                            INNER JOIN geo.location l2
                                ON l2.id = il2.location_id
                            WHERE
                                il2.initiative_id = i.id
                                AND il2.location_id = @departmentId::int
                                AND l2.level = @departmentLevel
                        )
                    )

                    AND (
                        @initiativeId::int IS NULL
                        OR il.initiative_id = @initiativeId::int
                    )
            ) q
        """;

        var parameters = new[]
        {
            new NpgsqlParameter("departmentId", departmentId ?? (object)DBNull.Value),
            new NpgsqlParameter("initiativeId", initiativeId ?? (object)DBNull.Value),
            new NpgsqlParameter("departmentLevel", (byte)LocationLevel.Department),
        };

        var area = await dbContext.Database
            .SqlQueryRaw<double>(sql, parameters)
            .SingleOrDefaultAsync(ct);

        return area / 1000000;
    }

    /// <inheritdoc/>
    public async Task<int> GetPeopleInvolvedCountAsync(int? departmentId = null, int? initiativeId = null, CancellationToken ct = default) =>
        await dbContext.InitiativeUsers
            .Where(e => e.Initiative.Enabled &&
                (departmentId == null ||
                    e.Initiative.MainLocationId == departmentId ||
                    e.Initiative.InitiativeLocations.Any(e => e.LocationId == departmentId &&
                        e.Location.Level == (byte)LocationLevel.Department)) &&
                (initiativeId == null || e.InitiativeId == initiativeId))
            .Select(e => e.UserName)
            .Distinct()
            .CountAsync(ct);

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

    /// <inheritdoc/>
    public async Task<double> CalcAreaAsync(Initiative entity, CancellationToken ct = default)
    {
        // If initiative has a polygon, calculate its area
        if (entity.Polygon != null && !entity.Polygon.IsEmpty)
        {
            return GeometryUtils.CalculatePolygonAreaInSquareKilometers(entity.Polygon as Polygon);
        }

        // If doesn´t have a polygon, calculate area from municipalities
        if (entity.InitiativeLocations != null && entity.InitiativeLocations.Count > 0)
        {
            var locationIds = entity.InitiativeLocations
                .Select(il => il.LocationId)
                .ToArray();

            var municipalities = await dbContext.LocationPolygons
                .Include(e => e.Location)
                    .ThenInclude(e => e.InitiativeLocations)
                .Where(e => e.Geometry != null &&
                    !e.Geometry.IsEmpty &&
                    e.Location.Level == (byte)LocationLevel.Municipality &&
                    locationIds.Contains(e.LocationId))
                .Select(e => e.Geometry)
                .ToListAsync(ct);

            return municipalities
                .Select(GeometryUtils.CalculateAreaInSquareKilometers)
                .Sum();
        }

        return 0;
    }
}
