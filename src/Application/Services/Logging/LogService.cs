namespace IAVH.BioTablero.CM.Application.Services.Logging;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Logging;
using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Application.Mappings;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Logging;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.OData;

/// <summary>
/// System logs service.
/// </summary>
public class LogService : ServiceRead<LogEntity, LogDto, Guid>, ILogService
{
    private const int ReportMaxRows = 10000;
    private new readonly ILogRepository entityRepository;
    private readonly IReportService<LogDto> entityReportService;

    /// <summary>
    /// Entity mapper.
    /// </summary>
    private readonly IMapper<LogEntity, LogBaseDto> odataMapper;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="entityReportService">Entity report service.</param>
    public LogService(
        ILogRepository entityRepository,
        IMapper<LogEntity, LogDto> mapper,
        IReportService<LogDto> entityReportService)
        : base(entityRepository, mapper)
    {
        this.entityRepository = entityRepository;
        this.entityReportService = entityReportService;
        odataMapper = new LogBaseMappings();
    }

    /// <summary>
    /// Get elements list (OData).
    /// </summary>
    /// <param name="queryOptions">OData query options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public override async Task<CustomWebResponse> GetListAsync(ODataQueryOptions<LogEntity> queryOptions, CancellationToken ct = default)
    {
        var query = entityRepository.GetQueryable();
        query = entityRepository.IncludeOdataFilters(query);

        try
        {
            var odataResponse = await GetOdataDtoListByQueryAsync(query, queryOptions, ct);
            return GetOdataWebResponse(odataResponse, odataMapper);
        }
        catch (ODataException ex)
        {
            return new(true)
            {
                Message = $"Invalid filter: {ex.Message}",
            };
        }
    }

    /// <summary>
    /// Generate Excel report.
    /// </summary>
    /// <param name="queryOptions">OData query options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public async Task<CustomWebResponse> GenerateExcel(ODataQueryOptions<LogEntity> queryOptions, CancellationToken ct = default)
    {
        var query = entityRepository.GetQueryable();
        query = entityRepository.IncludeOdataFilters(query);

        try
        {
            query = AddOdataQueryFilterAndOrder(query, queryOptions, DefaultOdataQuerySettings);

            var totalItems = await entityRepository.QueryCountAsync(query, ct);

            if (totalItems > ReportMaxRows)
            {
                return new(true)
                {
                    Message = $"The parameters generate a file with more than {ReportMaxRows} records.",
                };
            }

            // Get result
            var dataList = await entityRepository.QueryToListAsync(query, ct);

            // Map DTO list
            var dataListDto = dataList
                .Select(mapper.Map)
                .ToArray();

            return new()
            {
                ResponseBody = entityReportService.GenerateReport(dataListDto),
            };
        }
        catch (ODataException ex)
        {
            return new(true)
            {
                Message = $"Invalid filter: {ex.Message}",
            };
        }
    }
}
