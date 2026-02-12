namespace IAVH.BioTablero.CM.Application.Services.General;

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Core.Domain.Entities;
using IAVH.BioTablero.CM.Core.Domain.Models.Validations;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.OData;

using ODataUtilsCustom = IAVH.BioTablero.CM.Application.Utils.ODataUtils;

/// <summary>
/// General service for only read functions.
/// </summary>
/// <typeparam name="TE">Entity type.</typeparam>
/// <typeparam name="TDto">DTO class type.</typeparam>
/// <typeparam name="TI">Entity identifier type.</typeparam>
/// <param name="entityRepository">Entity repository.</param>
/// <param name="mapper">Entity mapper.</param>
/// <param name="errorTranslator">Error translator.</param>
public abstract class ServiceRead<TE, TDto, TI>(
    IRepository<TE, TI> entityRepository,
    IMapperRead<TE, TDto> mapper,
    IValidationErrorTranslator errorTranslator) : IRead<TE, TI>
    where TDto : class, IDto
    where TE : BaseEntity<TI>, IAggregateRoot
{
    /// <summary>
    /// Entity repository.
    /// </summary>
    private protected readonly IRepository<TE, TI> entityRepository = entityRepository;

    /// <summary>
    /// Entity mapper.
    /// </summary>
    private protected readonly IMapperRead<TE, TDto> mapper = mapper;

    /// <summary>
    /// Error translator.
    /// </summary>
    private protected readonly IValidationErrorTranslator errorTranslator = errorTranslator;

    /// <inheritdoc/>
    public virtual async Task<bool> AnyAsync(TI id, CancellationToken ct = default) => await entityRepository.AnyAsync(id, ct);

    /// <inheritdoc/>
    public virtual async Task<CustomWebResponse> GetItemAsync(TI id, CancellationToken ct = default)
    {
        var dataEntity = await entityRepository.GetByIdAsync(id, ct);

        if (dataEntity != null)
        {
            var dataDto = mapper.Map(dataEntity);
            return new()
            {
                ResponseBody = dataDto,
            };
        }
        else
        {
            return new(true)
            {
                StatusCode = HttpStatusCode.NotFound,
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.ElementNotFound, propertyName: nameof(TE)),
            };
        }
    }

    /// <inheritdoc/>
    public virtual async Task<CustomWebResponse> GetAllAsync(CancellationToken ct = default)
    {
        var dataListEntity = await entityRepository.ListAsync(ct);
        var dataListDto = dataListEntity
            .Select(mapper.Map);

        return new()
        {
            ResponseBody = dataListDto,
        };
    }

    /// <inheritdoc/>
    public virtual async Task<CustomWebResponse> GetListAsync(ODataQueryOptions<TE> queryOptions, CancellationToken ct = default)
    {
        var query = entityRepository.GetQueryable();
        return await GetOdataListByQueryAsync(query, queryOptions, ct);
    }

    /// <summary>
    /// Get OData object by custom Linq query.
    /// </summary>
    /// <param name="query">Linq Query.</param>
    /// <param name="queryOptions">OData query options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Object with total items and data list.</returns>
    private protected async Task<ODataResponse<TE>> GetOdataDtoListByQueryAsync(IQueryable<TE> query, ODataQueryOptions<TE> queryOptions, CancellationToken ct = default)
    {
        query = ODataUtilsCustom.AddOdataQueryFilterAndOrder(query, queryOptions);

        var totalItems = await entityRepository.QueryCountAsync(query, ct);

        query = ODataUtilsCustom.AddOdataQueryPagination(query, queryOptions);

        // Get result
        var dataList = await entityRepository.QueryToListAsync(query, ct);

        return new()
        {
            TotalItems = totalItems,
            DataList = dataList,
        };
    }

    /// <summary>
    /// Generate OData web response.
    /// </summary>
    /// <typeparam name="TDtoM">DTO class type (for mappings).</typeparam>
    /// <param name="odataResponse">OData response data.</param>
    /// <param name="responseMapper">Mapper for response.</param>
    /// <returns>Custom OData web response.</returns>
    private protected static CustomWebResponse GetOdataWebResponse<TDtoM>(ODataResponse<TE> odataResponse, IMapperRead<TE, TDtoM> responseMapper)
        where TDtoM : class, IDto => new()
        {
            ResponseBody = new Dictionary<string, object>()
            {
                ["@odata.count"] = odataResponse.TotalItems,
                ["value"] = odataResponse.DataList.Select(responseMapper.Map),
            },
        };

    /// <summary>
    /// Get OData list data by custom Linq query.
    /// </summary>
    /// <param name="query">Linq Query.</param>
    /// <param name="queryOptions">OData query options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    private protected async Task<CustomWebResponse> GetOdataListByQueryAsync(IQueryable<TE> query, ODataQueryOptions<TE> queryOptions, CancellationToken ct = default)
    {
        try
        {
            var odataResponse = await GetOdataDtoListByQueryAsync(query, queryOptions, ct);
            return GetOdataWebResponse(odataResponse, mapper);
        }
        catch (ODataException ex)
        {
            return new(true)
            {
                ResponseBody = errorTranslator.Translate(ValidationErrorCodes.General.OdataInvalidFilter, data: ex.StackTrace),
            };
        }
    }
}
