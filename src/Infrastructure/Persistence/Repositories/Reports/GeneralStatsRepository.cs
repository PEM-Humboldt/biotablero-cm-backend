namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Reports;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Tags;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Reports;

using Microsoft.EntityFrameworkCore;

using Npgsql;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.GeoEnums;

using TagCategoryEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.TagEnums.TagCategory;

/// <summary>
/// General Statistics repository.
/// </summary>
/// <param name="dbContext">General Database Context.</param>
public class GeneralStatsRepository(GeneralContext dbContext) : IGeneralStatsRepository
{
    /// <summary>
    /// General Database context.
    /// </summary>
    private protected readonly GeneralContext dbContext = dbContext;

    /// <inheritdoc/>
    public async Task<int> GetEnabledRecordsCountAsync(string userName = null, int? departmentId = null, int? initiativeId = null, CancellationToken ct = default) =>
        await dbContext.Initiatives
            .Where(e => e.Enabled &&
                (userName == null || e.InitiativeUsers.Any(e => e.UserName == userName)) &&
                (departmentId == null ||
                    e.MainLocationId == departmentId ||
                    e.InitiativeLocations.Any(e =>
                        (e.LocationId == departmentId && e.Location.Level == (byte)LocationLevel.Department) ||
                        (e.Location.ParentId == departmentId && e.Location.Level == (byte)LocationLevel.Municipality))) &&
                (initiativeId == null || e.Id == initiativeId))
            .Distinct()
            .CountAsync(ct);

    /// <inheritdoc/>
    public async Task<double> GetAreaAsync(int? departmentId = null, int? initiativeId = null, CancellationToken ct = default)
    {
        var initiativeCustomPolygonsSql = """
            SELECT DISTINCT i.polygon AS geom
            FROM initiatives.initiative i
            WHERE
                i.polygon IS NOT NULL
                AND i.enabled = @enabledInitiatives
                AND i.polygon_area > 0

                AND (
                    @departmentId::int IS NULL
                    OR i.main_location_id = @departmentId::int

                    OR EXISTS (
                        SELECT 1
                        FROM initiatives.initiative_location il2
                        LEFT JOIN geo.location dept ON dept.id = il2.location_id
                        LEFT JOIN geo.location mun ON mun.id = il2.location_id
                        WHERE
                            il2.initiative_id = i.id
                            AND (
                                (dept.level = @departmentLevel AND dept.id = @departmentId)
                                OR (mun.level = @municipalityLevel AND mun.parent_id = @departmentId)
                            )
                    )
                )

                AND (
                    @initiativeId::int IS NULL
                    OR i.id = @initiativeId::int
                )
        """;

        var initiativeLocationsSql = """
            SELECT DISTINCT lp.geometry AS geom
            FROM initiatives.initiative_location il
            INNER JOIN initiatives.initiative i
                ON i.id = il.initiative_id
            INNER JOIN geo.location l
                ON l.id = il.location_id
            INNER JOIN geo.location_polygon lp
                ON lp.location_id = l.id
            WHERE
                i.polygon IS NULL
                AND i.enabled = @enabledInitiatives
                AND i.polygon_area > 0

                AND (
                    @departmentId::int IS NULL
                    OR i.main_location_id = @departmentId::int

                    OR EXISTS (
                        SELECT 1
                        FROM initiatives.initiative_location il2
                        LEFT JOIN geo.location dept ON dept.id = il2.location_id
                        LEFT JOIN geo.location mun ON mun.id = il2.location_id
                        WHERE
                            il2.initiative_id = i.id
                            AND (
                                (dept.level = @departmentLevel AND dept.id = @departmentId)
                                OR (mun.level = @municipalityLevel AND mun.parent_id = @departmentId)
                            )
                    )
                )

                AND (
                    @initiativeId::int IS NULL
                    OR il.initiative_id = @initiativeId::int
                )
        """;

        var sql = $"""
            SELECT
                COALESCE(
                    ST_Area(
                        ST_Transform(
                            ST_UnaryUnion(geom),
                            3116
                        )
                    ) / 1000000.0,
                    0
                ) AS "Value"
            FROM (
                {initiativeCustomPolygonsSql}

                UNION ALL

                {initiativeLocationsSql}
            ) q
        """;

        var parameters = new[]
        {
            new NpgsqlParameter("enabledInitiatives", true),
            new NpgsqlParameter("departmentLevel", (byte)LocationLevel.Department),
            new NpgsqlParameter("municipalityLevel", (byte)LocationLevel.Municipality),
            new NpgsqlParameter("departmentId", departmentId ?? (object)DBNull.Value),
            new NpgsqlParameter("initiativeId", initiativeId ?? (object)DBNull.Value),
        };

        return await dbContext.Database
            .SqlQueryRaw<double>(sql, parameters)
            .SingleOrDefaultAsync(ct);
    }

    /// <inheritdoc/>
    public async Task<int> GetPeopleInvolvedCountAsync(int? departmentId = null, int? initiativeId = null, CancellationToken ct = default) =>
        await dbContext.Initiatives
            .Where(e => e.Enabled &&
                (departmentId == null ||
                    e.MainLocationId == departmentId ||
                    e.InitiativeLocations.Any(e =>
                        (e.LocationId == departmentId && e.Location.Level == (byte)LocationLevel.Department) ||
                        (e.Location.ParentId == departmentId && e.Location.Level == (byte)LocationLevel.Municipality))) &&
                (initiativeId == null || e.Id == initiativeId))
            .SelectMany(e => e.InitiativeUsers.Select(e => e.UserName))
            .Distinct()
            .CountAsync(ct);

    /// <inheritdoc/>
    public async Task<int> GetAgreementsInvolvedCountAsync(int? departmentId = null, int? initiativeId = null, CancellationToken ct = default) =>
        await dbContext.Initiatives
            .Where(e => e.Enabled &&
                e.InitiativeTags.Any(e => e.Tag.CategoryId == (int)TagCategoryEnum.PoliticalContext || e.Tag.CategoryId == (int)TagCategoryEnum.SocialContext) &&
                (departmentId == null ||
                    e.MainLocationId == departmentId ||
                    e.InitiativeLocations.Any(e =>
                        (e.LocationId == departmentId && e.Location.Level == (byte)LocationLevel.Department) ||
                        (e.Location.ParentId == departmentId && e.Location.Level == (byte)LocationLevel.Municipality))) &&
                (initiativeId == null || e.Id == initiativeId))
            .SelectMany(e => e.InitiativeTags.Select(e => e.TagId))
            .Distinct()
            .CountAsync(ct);

    /// <inheritdoc/>
    public async Task<List<Tag>> GetEcosystemsAsync(int? departmentId, int? initiativeId, CancellationToken ct = default) =>
        await dbContext.Initiatives
            .Where(e => e.Enabled &&
                e.InitiativeTags.Any(e => e.Tag.CategoryId == (int)TagCategoryEnum.Ecosystem) &&
                (departmentId == null ||
                    e.MainLocationId == departmentId ||
                    e.InitiativeLocations.Any(e =>
                        (e.LocationId == departmentId && e.Location.Level == (byte)LocationLevel.Department) ||
                        (e.Location.ParentId == departmentId && e.Location.Level == (byte)LocationLevel.Municipality))) &&
                (initiativeId == null || e.Id == initiativeId))
            .SelectMany(e => e.InitiativeTags.Select(e => e.Tag))
            .Distinct()
            .ToListAsync(ct);

    /// <inheritdoc/>
    public async Task<List<string>> GetUserNamesAsync(int? departmentId, int? initiativeId, CancellationToken ct = default) =>
        await dbContext.InitiativeUsers
            .Where(e => (departmentId == null ||
                    e.Initiative.InitiativeLocations.Any(e =>
                        (e.LocationId == departmentId && e.Location.Level == (byte)LocationLevel.Department) ||
                        (e.Location.ParentId == departmentId && e.Location.Level == (byte)LocationLevel.Municipality))) &&
                (initiativeId == null || e.Initiative.Id == initiativeId))
            .Select(e => e.UserName)
            .Distinct()
            .ToListAsync(ct);

    /// <inheritdoc/>
    public async Task<List<KeyValuePair<string, int>>> GetIndicatorsByScaleAsync(int? departmentId, int? initiativeId, CancellationToken ct = default) =>
        await dbContext.IndicatorTags
            .Where(e => e.Tag.CategoryId == (int)TagCategoryEnum.BiologicalGroup &&
                (departmentId == null ||
                    e.Indicator.IndicatorLocations.Any(e =>
                        (e.LocationId == departmentId && e.Location.Level == (byte)LocationLevel.Department) ||
                        (e.Location.ParentId == departmentId && e.Location.Level == (byte)LocationLevel.Municipality))) &&
                (initiativeId == null || e.Indicator.InitiativeId == initiativeId))
            .GroupBy(e => e.Tag.Name)
            .Select(e => new KeyValuePair<string, int>(e.Key, e.Count()))
            .ToListAsync(ct);
}
