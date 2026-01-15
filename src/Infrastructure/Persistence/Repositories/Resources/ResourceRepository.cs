namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Resources;

using IAVH.BioTablero.CM.Core.Domain.Entities.Resources;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Resources;

/// <summary>
/// Resource repository.
/// </summary>
public class ResourceRepository : Repository<Resource, int>, IResourceRepository
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    public ResourceRepository(GeneralContext dbContext)
        : base(dbContext)
    {
    }
}
