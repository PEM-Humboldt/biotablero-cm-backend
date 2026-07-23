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
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1304:Specify CultureInfo", Justification = "Avoid exception when transforming code to SQL")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1862:Use the 'StringComparison' method overloads to perform case-insensitive string comparisons", Justification = "Avoid exception when transforming code to SQL")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1311:Specify a culture or use an invariant version", Justification = "Avoid exception when transforming code to SQL")]
    public async Task<bool> IsDuplicated(string name, int categoryId, CancellationToken ct = default) =>
        await dbContext.Tags
            .Where(e => e.Name.ToLower() == name.ToLower() && e.CategoryId == categoryId)
            .AnyAsync(ct);

    /// <inheritdoc/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1304:Specify CultureInfo", Justification = "Avoid exception when transforming code to SQL")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1862:Use the 'StringComparison' method overloads to perform case-insensitive string comparisons", Justification = "Avoid exception when transforming code to SQL")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1311:Specify a culture or use an invariant version", Justification = "Avoid exception when transforming code to SQL")]
    public async Task<bool> IsDuplicated(int id, string name, int categoryId, CancellationToken ct = default) =>
        await dbContext.Tags
            .Where(e => e.Id != id && e.Name.ToLower() == name.ToLower() && e.CategoryId == categoryId)
            .AnyAsync(ct);
}
