namespace IAVH.BioTablero.CM.Application.Interfaces.Services;

using System;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Logging;

using Microsoft.AspNetCore.OData.Query;

/// <summary>
/// System logs service interface.
/// </summary>
public interface ILogService : IRead<LogEntity, Guid>
{
    /// <summary>
    /// Generate Excel report.
    /// </summary>
    /// <param name="queryOptions">OData query options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> GenerateExcel(ODataQueryOptions<LogEntity> queryOptions, CancellationToken ct = default);
}
