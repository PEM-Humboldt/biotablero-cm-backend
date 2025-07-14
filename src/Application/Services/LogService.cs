namespace IAVH.BioTablero.CM.Application.Services;

using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Application.Specifications;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.DTOs.LogNS;
using IAVH.BioTablero.CM.Core.Entities.LogNS;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OData.Query;
using System.Threading;
using System.Linq;
using IAVH.BioTablero.CM.Core.Helpers.General;
using System.Collections.Generic;


public class LogService(IRepository<LogEntity> entityRepository,
    IMapper<LogEntity, LogDto> mapper) : ServiceRead<LogEntity, LogDto, Guid, LogSpec>(entityRepository, mapper), ILogService
{
    /// <summary>
    /// Get elements list (OData)
    /// </summary>
    /// <param name="baseUrl">API base url</param>
    /// <param name="queryOptions">OData query options</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    public virtual async Task<CustomWebResponse> GetList(string baseUrl, ODataQueryOptions<LogDto> queryOptions, CancellationToken ct = default)
    {
        // Get OData parameters
        var skip = queryOptions.Skip?.Value ?? 0;
        var top = queryOptions.Top?.Value ?? 20;

        var spec = new LogSpec(skip, top);
        var dataListEntity = await entityRepository.ListAsync(spec, ct);
        var totalCount = await entityRepository.CountAsync(ct);

        var dataListDto = dataListEntity
            .Select(mapper.Map);

        Uri nextLink = null;
        if (skip + top < totalCount)
        {
            nextLink = new Uri($"{baseUrl}?$skip={skip + top}&$top={top}");
        }

        return new()
        {
            ResponseBody = new Dictionary<string, object>
            {
                ["@odata.count"] = totalCount,
                ["value"] = dataListDto,
                ["@odata.nextLink"] = nextLink,
            }
        };
    }
}
