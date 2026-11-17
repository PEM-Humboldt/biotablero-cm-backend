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

/// <summary>
/// Central actor repository.
/// </summary>
/// <param name="dbContext">General Database Context.</param>
/// <param name="logger">System logger.</param>
public class CentralActorRepository(
    GeneralContext dbContext,
    ILogger logger) : Repository<CentralActor, int>(dbContext, logger), ICentralActorRepository
{
    /// <inheritdoc/>
    public async Task<IEnumerable<CentralActor>> GetByNamesAsync(string[] names, CancellationToken ct = default) =>
        await dbContext.CentralActors
            .Where(e => names.Contains(e.Name))
            .ToListAsync(ct);
}
