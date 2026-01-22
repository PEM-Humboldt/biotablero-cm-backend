namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Tags;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Tags;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Tags;

using Microsoft.EntityFrameworkCore;

using Serilog;

/// <summary>
/// Tag repository.
/// </summary>
public class TagRepository : Repository<Tag, int>, ITagRepository
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    /// <param name="logger">System logger.</param>
    public TagRepository(
        GeneralContext dbContext,
        ILogger logger)
        : base(dbContext, logger)
    {
    }

    /// <inheritdoc/>
    public async Task<bool> AnyByNameAndCategory(string name, int categoryId, CancellationToken ct = default) =>
        await dbContext.Tags
            .Where(e => e.Name == name && e.CategoryId == categoryId)
            .AnyAsync(ct);

    /// <inheritdoc/>
    public async Task<bool> IsDuplicated(int id, string name, int categoryId, CancellationToken ct = default) =>
        await dbContext.Tags
            .Where(e => e.Id != id && e.Name == name && e.CategoryId == categoryId)
            .AnyAsync(ct);
}
