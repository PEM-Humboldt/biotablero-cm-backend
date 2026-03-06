namespace IAVH.BioTablero.CM.Application.Services.Logging;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.DTOs.Logging;
using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Logging;
using IAVH.BioTablero.CM.Application.Mappings.Logging;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Logging;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Logging;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.OData;

using ODataUtilsCustom = IAVH.BioTablero.CM.Application.Utils.ODataUtils;

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
    private readonly IMapperRead<LogEntity, LogBaseDto> odataMapper;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="errorTranslator">Error translator.</param>
    /// <param name="entityReportService">Entity report service.</param>
    public LogService(
        ILogRepository entityRepository,
        IMapperRead<LogEntity, LogDto> mapper,
        IValidationErrorTranslator errorTranslator,
        IReportService<LogDto> entityReportService)
        : base(entityRepository, mapper, errorTranslator)
    {
        this.entityRepository = entityRepository;
        this.entityReportService = entityReportService;
        odataMapper = new LogBaseMappings();
    }

    /// <inheritdoc/>
    public override async Task<CustomWebResponse> GetListAsync(ODataQueryOptions<LogEntity> queryOptions, CancellationToken ct = default)
    {
        var query = entityRepository.GetQueryable();
        query = entityRepository.IncludeOdataFilters(query);

        try
        {
            var odataResponse = await GetOdataDtoListByQueryAsync(query, queryOptions, ct);
            return GetOdataWebResponse(odataResponse, odataMapper);
        }
        catch (ODataException)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.OdataInvalidFilter),
            };
        }
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GenerateExcel(ODataQueryOptions<LogEntity> queryOptions, CancellationToken ct = default)
    {
        var query = entityRepository.GetQueryable();
        query = entityRepository.IncludeOdataFilters(query);

        try
        {
            query = ODataUtilsCustom.AddOdataQueryFilterAndOrder(query, queryOptions);

            var totalItems = await entityRepository.QueryCountAsync(query, ct);

            if (totalItems > ReportMaxRows)
            {
                return new(true)
                {
                    ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.OdataRowLimitExceeded, data: ReportMaxRows),
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
        catch (ODataException)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.OdataInvalidFilter),
            };
        }
    }
}
