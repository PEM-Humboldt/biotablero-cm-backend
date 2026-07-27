namespace IAVH.BioTablero.CM.Application.Services.Indicators;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.DTOs.Indicators;
using IAVH.BioTablero.CM.Application.Interfaces.ExternalServices.Spreadsheets.Services;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Interfaces.Services.Indicators;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Indicators;
using IAVH.BioTablero.CM.Core.Interfaces.ExternalServices;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories.Indicators;

using Microsoft.AspNetCore.OData.Query;

/// <summary>
/// Indicator service.
/// </summary>
public class IndicatorService : ServiceRead<Indicator, IndicatorDto, int>, IIndicatorService
{
    private new readonly IIndicatorRepository entityRepository;
    private readonly IIndicatorExcelService excelService;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entityRepository">Entity repository.</param>
    /// <param name="mapper">Entity mapper.</param>
    /// <param name="errorTranslator">Error translator.</param>
    /// <param name="excelService">Excel service.</param>
    public IndicatorService(
        IIndicatorRepository entityRepository,
        IMapperRead<Indicator, IndicatorDto> mapper,
        IValidationErrorTranslator errorTranslator,
        IIndicatorExcelService excelService)
    : base(entityRepository, mapper, errorTranslator)
    {
        this.entityRepository = entityRepository;
        this.excelService = excelService;
    }

    /// <inheritdoc/>
    public override async Task<CustomWebResponse> GetListAsync(ODataQueryOptions<Indicator> queryOptions, CancellationToken ct = default)
    {
        var query = entityRepository.GetQueryable();
        query = entityRepository.IncludeOdataEntities(query);

        return await GetOdataListByQueryAsync(query, queryOptions, ct);
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> GetByInitiativeAsync(int initiativeId, CancellationToken ct = default)
    {
        var dataListEntity = await entityRepository.GetByInitiativeAsync(initiativeId, ct);

        var dataListDto = dataListEntity
            .Select(mapper.Map);

        return new()
        {
            ResponseBody = dataListDto,
        };
    }

    /// <inheritdoc/>
    public async Task<CustomWebResponse> ImportIndicatorsAsync(string userName, IndicatorsImportFileDto requestDataDto, IInputFile formFile, CancellationToken ct)
    {
        var fileReadResult = excelService.GetFileData(formFile);

        if (fileReadResult.Errors.Count > 0)
        {
            return new(true)
            {
                ResponseBody = fileReadResult.Errors,
            };
        }

        return new();
    }
}
