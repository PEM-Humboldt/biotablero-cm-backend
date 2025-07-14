using System;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.DTOs.LogNS;
using IAVH.BioTablero.CM.Core.Helpers.General;

using Microsoft.AspNetCore.OData.Query;

namespace IAVH.BioTablero.CM.Application.Interfaces.Services;

public interface ILogService : IServiceRead<LogDto, Guid>
{
    /// <summary>
    /// Get elements list (OData)
    /// </summary>
    /// <param name="queryOptions">OData query options</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    public Task<CustomWebResponse> GetList(ODataQueryOptions<LogDto> queryOptions, CancellationToken ct = default);
}