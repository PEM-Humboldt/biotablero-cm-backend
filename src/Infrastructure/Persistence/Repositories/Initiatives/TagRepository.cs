namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Initiatives;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Initiatives;


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

    /// <summary>
    /// Get if elements exists by name.
    /// </summary>
    /// <param name="name">Entity name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    public async Task<bool> AnyByName(string name, CancellationToken ct = default) =>
        await dbContext.Tags
            .Where(e => e.Name == name)
            .AnyAsync(ct);

    /// <summary>
    /// Get if element is duplicated.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="name">Entity name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if any element exists. False otherwise.</returns>
    public async Task<bool> IsDuplicated(int id, string name, CancellationToken ct = default) =>
        await dbContext.Tags
            .Where(e => e.Id != id && e.Name == name)
            .AnyAsync(ct);
}
