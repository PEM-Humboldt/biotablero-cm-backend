namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Tags;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Tags;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Tags;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Tag repository.
/// </summary>
public class TagRepository : Repository<Tag, int>, ITagRepository
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    public TagRepository(GeneralContext dbContext)
        : base(dbContext)
    {
    }

    /// <inheritdoc/>
    public async Task<bool> AnyByName(string name, CancellationToken ct = default) =>
        await dbContext.Tags
            .Where(e => e.Name == name)
            .AnyAsync(ct);

    /// <inheritdoc/>
    public async Task<bool> IsDuplicated(int id, string name, CancellationToken ct = default) =>
        await dbContext.Tags
            .Where(e => e.Id != id && e.Name == name)
            .AnyAsync(ct);
}
