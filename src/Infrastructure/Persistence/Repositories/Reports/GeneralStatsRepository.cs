namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Reports;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Tags;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Reports;

using Microsoft.EntityFrameworkCore;

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
}
