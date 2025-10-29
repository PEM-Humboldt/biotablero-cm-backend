namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Logging;

using System.Linq;

using IAVH.BioTablero.CM.Core.Domain.Entities.Logging;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

/// <summary>
/// Custom Log repository.
/// </summary>
/// <param name="dbContext">General Database Context.</param>
public class LogRepository(GeneralContext dbContext) : Repository<LogEntity>(dbContext), ILogRepository
{
    /// <summary>
    /// Include OData custom filters.
    /// </summary>
    /// <param name="query">Linq Query.</param>
    /// <returns>Modified Linq query.</returns>
    public IQueryable<LogEntity> IncludeOdataFilters(IQueryable<LogEntity> query) =>
        query
            .Where(e => e.CustomRecord)
            .Select(e => new LogEntity()
            {
                Id = e.Id,
                TimeStamp = e.TimeStamp,
                Type = e.Type,
                Message = e.Message,
                UserName = e.UserName,
                ClientAgent = e.ClientAgent,
                ClientIp = e.ClientIp,
            });
}
