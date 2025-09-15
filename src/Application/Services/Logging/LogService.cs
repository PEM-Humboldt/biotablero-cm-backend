namespace IAVH.BioTablero.CM.Application.Services.Logging;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Logging;
using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Specifications;
using IAVH.BioTablero.CM.Core.Domain.Entities.Logging;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.OData;

/// <summary>
/// System logs service.
/// </summary>
public class LogService(IRepository<LogEntity> entityRepository,
    IMapper<LogEntity, LogDto> mapper,
    IReportService<LogDto> entityReportService) : ServiceRead<LogEntity, LogDto, Guid, LogSpec>(entityRepository, mapper), ILogService
{
    /// <summary>
    /// Generate Excel report.
    /// </summary>
    /// <param name="queryOptions">OData query options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<byte[]> GenerateExcel(ODataQueryOptions<LogEntity> queryOptions, CancellationToken ct)
    {
        var query = entityRepository.GetQueryable();

        var defaultSettings = new ODataQuerySettings()
        {
            HandleNullPropagation = HandleNullPropagationOption.True,
        };

        try
        {
            query = AddOdataQueryFilterAndOrder(query, queryOptions, defaultSettings);

            var totalItems = await entityRepository.QueryCountAsync(query, ct);

            // Get result
            var dataList = await entityRepository.QueryToListAsync(query, ct);

            // Map DTO list
            var dataListDto = dataList
                .Select(mapper.Map)
                .ToList();

            return entityReportService.GenerateReport(dataListDto);
        }
        catch (ODataException)
        {
            return null;
        }
    }
}
