namespace IAVH.BioTablero.CM.Application.Services.General;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Specifications;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.OData;

/// <summary>
/// General service for only read functions.
/// </summary>
/// <typeparam name="TE">Entity type.</typeparam>
/// <typeparam name="TDto">DTO class type.</typeparam>
/// <typeparam name="TI">Entity identifier type.</typeparam>
/// <typeparam name="TS">General Specification type.</typeparam>
/// <remarks>
/// Initialize service.
/// </remarks>
public abstract class ServiceRead<TE, TDto, TI, TS>(IRepository<TE> entityRepository, IMapper<TE, TDto> mapper) : IServiceRead<TE, TDto, TI>
    where TDto : class, IDto
    where TI : notnull
    where TE : class, IAggregateRoot
    where TS : GeneralSpecification<TI, TE>
{
    /// <summary>
    /// Entity repository.
    /// </summary>
    private protected readonly IRepository<TE> entityRepository = entityRepository;

    /// <summary>
    /// Entity mapper.
    /// </summary>
    private protected readonly IMapper<TE, TDto> mapper = mapper;

    /// <summary>
    /// Check if element exists.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public virtual async Task<bool> Exists(TI id, CancellationToken ct = default)
    {
        var specification = (TS)Activator.CreateInstance(typeof(TS), [id]);
        return await entityRepository.AnyAsync(specification, ct);
    }

    /// <summary>
    /// Get element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public virtual async Task<CustomWebResponse> GetItem(TI id, CancellationToken ct = default)
    {
        var specification = (TS)Activator.CreateInstance(typeof(TS), [id]);
        var dataEntity = await entityRepository.FirstOrDefaultAsync(specification, ct);

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
                Message = "Not found",
            };
        }
    }

    /// <summary>
    /// Get all elements.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public virtual async Task<CustomWebResponse> GetAll(CancellationToken ct = default)
    {
        var dataListEntity = await entityRepository.ListAsync(ct);
        var dataListDto = dataListEntity
            .Select(mapper.Map);

        return new()
        {
            ResponseBody = dataListDto,
        };
    }

    /// <summary>
    /// Get elements list (paginated).
    /// </summary>
    /// <param name="skip">Page.</param>
    /// <param name="take">Page size.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public virtual async Task<CustomWebResponse> GetList(int skip, int take, CancellationToken ct = default)
    {
        var specification = (TS)Activator.CreateInstance(typeof(TS), [skip, take]);
        var dataListEntity = await entityRepository.ListAsync(specification, ct);
        var dataListDto = dataListEntity
            .Select(mapper.Map);

        return new()
        {
            ResponseBody = dataListDto,
        };
    }

    /// <summary>
    /// Get elements list (OData).
    /// </summary>
    /// <param name="queryOptions">OData query options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public virtual async Task<CustomWebResponse> GetList(ODataQueryOptions<TE> queryOptions, CancellationToken ct = default)
    {
        var query = entityRepository.GetQueryable();
        return await GetOdataListByQuery(query, queryOptions, ct);
    }

    /// <summary>
    /// Get OData list data by custom Linq query.
    /// </summary>
    /// <param name="query">Linq SQL Query.</param>
    /// <param name="queryOptions">OData query options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    private protected async Task<CustomWebResponse> GetOdataListByQuery(IQueryable<TE> query, ODataQueryOptions<TE> queryOptions, CancellationToken ct = default)
    {
        var defaultSettings = new ODataQuerySettings()
        {
            HandleNullPropagation = HandleNullPropagationOption.True,
        };

        try
        {
            AddOdataQueryFilterAndOrder(query, queryOptions, defaultSettings);

            var totalItems = await entityRepository.QueryCountAsync(query, ct);

            AddOdataQueryPagination(query, queryOptions, defaultSettings);

            // Get result
            var dataList = await entityRepository.QueryToListAsync(query, ct);

            return new()
            {
                ResponseBody = new Dictionary<string, object>()
                {
                    ["@odata.count"] = totalItems,
                    ["value"] = dataList.Select(mapper.Map),
                },
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

    /// <summary>
    /// Add filter and order settings to OData query.
    /// </summary>
    /// <param name="query">Linq SQL Query.</param>
    /// <param name="queryOptions">OData query options.</param>
    /// <param name="settings">OData query settings.</param>
    private protected static void AddOdataQueryFilterAndOrder(IQueryable<TE> query, ODataQueryOptions<TE> queryOptions, ODataQuerySettings settings)
    {
        // Apply order and filter settings
        if (queryOptions?.Filter != null)
        {
            queryOptions.Filter.ApplyTo(query, settings);
        }

        if (queryOptions.OrderBy != null)
        {
            queryOptions.OrderBy.ApplyTo(query, settings);
        }
    }

    /// <summary>
    /// Add pagination settings to OData query.
    /// </summary>
    /// <param name="query">Linq SQL Query.</param>
    /// <param name="queryOptions">OData query options.</param>
    /// <param name="settings">OData query settings.</param>
    private protected static void AddOdataQueryPagination(IQueryable<TE> query, ODataQueryOptions<TE> queryOptions, ODataQuerySettings settings)
    {
        const int maxPageSize = 20;

        // Apply pagination settings ($skip and $top)
        var pageSize = queryOptions.Top?.Value ?? maxPageSize;

        if (pageSize > maxPageSize)
        {
            pageSize = maxPageSize;
        }

        query = query.Take(pageSize);

        if (queryOptions.Skip != null)
        {
            queryOptions.Skip.ApplyTo(query, settings);
        }
    }
}
