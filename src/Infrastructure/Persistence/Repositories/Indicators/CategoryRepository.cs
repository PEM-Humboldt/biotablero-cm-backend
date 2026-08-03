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

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.IndicatorsEnums;

/// <summary>
/// Category repository.
/// </summary>
public class CategoryRepository : Repository<Category, int>, ICategoryRepository
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    /// <param name="logger">System logger.</param>
    public CategoryRepository(
        GeneralContext dbContext,
        ILogger logger)
        : base(dbContext, logger)
    {
    }

    /// <inheritdoc/>
    public override async Task<Category> GetByIdAsync(int id, CancellationToken ct = default) =>
        await dbContext.Categories
            .Include(e => e.Parent)
            .FirstOrDefaultAsync(ct);

    /// <inheritdoc/>
    public override async Task<List<Category>> ListAsync(CancellationToken ct = default) =>
        await dbContext.Categories
            .Include(e => e.Parent)
            .ToListAsync(ct);

    /// <inheritdoc/>
    public async Task<List<Category>> GetUpperGroupsAsync(string[] categoryNames, CancellationToken ct) =>
        await dbContext.Categories
            .Where(e => (e.ParentId == null || e.ParentId == (int)IndicatorBaseCategory.Species) && categoryNames.Contains(e.Name))
            .ToListAsync(ct);

    /// <inheritdoc/>
    public async Task<List<Category>> GetByParentsAsync(int[] parentsIds, CancellationToken ct) =>
        await dbContext.Categories
            .Include(e => e.Parent)
            .Where(e => e.ParentId != null && parentsIds.Contains(e.ParentId.Value))
            .ToListAsync(ct);
}
