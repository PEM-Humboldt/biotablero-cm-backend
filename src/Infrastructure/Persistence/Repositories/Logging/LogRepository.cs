namespace IAVH.BioTablero.CM.Infrastructure.Persistence.Repositories.Logging;

using System;
using System.Linq;

using IAVH.BioTablero.CM.Core.Domain.Entities.Logging;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Logging;

using Serilog;

/// <summary>
/// Log repository.
/// </summary>
/// <param name="dbContext">General Database Context.</param>
/// <param name="logger">System logger.</param>
public class LogRepository(GeneralContext dbContext, ILogger logger) : Repository<LogEntity, Guid>(dbContext, logger), ILogRepository
{
    /// <inheritdoc/>
    public IQueryable<LogEntity> IncludeOdataFilters(IQueryable<LogEntity> query) =>
        query
            .Where(e => e.CustomRecord)
            .Select(e => new LogEntity()
            {
                Id = e.Id,
                TimeStamp = e.TimeStamp,
                Type = e.Type,
                ShortMessage = e.ShortMessage,
                Message = e.Message,
                UserName = e.UserName,
                ClientAgent = e.ClientAgent,
                ClientIp = e.ClientIp,
            });
}
