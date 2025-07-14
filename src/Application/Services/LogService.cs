namespace IAVH.BioTablero.CM.Application.Services;

using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Application.Specifications;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.DTOs.LogNS;
using IAVH.BioTablero.CM.Core.Entities.LogNS;
using System;
using System.Threading.Tasks;
using IAVH.BioTablero.CM.Core.Helpers.General;
using Microsoft.AspNetCore.OData.Query;
using System.Threading;
using System.Linq;

public class LogService(IRepository<LogEntity> entityRepository,
    IMapper<LogEntity, LogDto> mapper) : ServiceRead<LogEntity, LogDto, Guid, LogSpec>(entityRepository, mapper), ILogService
{
    /// <summary>
    /// Get elements list (OData)
    /// </summary>
    /// <param name="queryOptions">OData query options</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    public virtual async Task<CustomWebResponse> GetList(ODataQueryOptions<LogDto> queryOptions, CancellationToken ct = default)
    {
        // Get OData parameters
        var skip = queryOptions.Skip?.Value ?? 0;
        var top = queryOptions.Top?.Value ?? 20;

        var spec = new LogSpec(skip, top);
        var dataListEntity = await entityRepository.ListAsync(spec, ct);

        var dataListDto = dataListEntity
            .Select(mapper.Map);

        return new()
        {
            ResponseBody = dataListDto
        };
    }
}
