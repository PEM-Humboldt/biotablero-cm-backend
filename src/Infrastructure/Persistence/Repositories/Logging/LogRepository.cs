namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Logging;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Entities.Logging;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using Microsoft.EntityFrameworkCore;

using NetTopologySuite.Geometries;

/// <summary>
/// Custom Log repository.
/// </summary>
public class LogRepository : Repository<LogEntity>, ILogRepository
{
    private readonly GeneralContext dbContext;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">General Database Context.</param>
    public LogRepository(GeneralContext dbContext)
        : base(dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <summary>
    /// Include OData custom filters.
    /// </summary>
    /// <param name="query">Linq Query.</param>
    /// <returns>Modified Linq query.</returns>
    public IQueryable<LogEntity> IncludeOdataFilters(IQueryable<LogEntity> query) =>
        query
            .Where(e => e.CustomRecord);
}
