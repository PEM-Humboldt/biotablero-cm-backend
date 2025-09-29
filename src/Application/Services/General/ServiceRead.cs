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
using IAVH.BioTablero.CM.Core.Domain.Entities;
using IAVH.BioTablero.CM.Core.Domain.Utils.Constants;
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
public abstract class ServiceRead<TE, TDto, TI, TS>(IRepository<TE> entityRepository, IMapper<TE, TDto> mapper) : IRead<TE, TI>
    where TDto : class, IDto
    where TI : notnull
    where TE : BaseEntity<TI>, IAggregateRoot
    where TS : GeneralSpecification<TI, TE>
{
    /// <summary>
    /// Default OData query settings.
    /// </summary>
    protected static readonly ODataQuerySettings DefaultOdataQuerySettings = new()
    {
        HandleNullPropagation = HandleNullPropagationOption.True,
    };

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
    public virtual async Task<bool> ExistsAsync(TI id, CancellationToken ct = default)
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
    public virtual async Task<CustomWebResponse> GetItemAsync(TI id, CancellationToken ct = default)
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
                Message = MessageConstants.NotFound,
            };
        }
    }

    /// <summary>
    /// Get all elements.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
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

    /// <summary>
    /// Get elements list (OData).
    /// </summary>
    /// <param name="queryOptions">OData query options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    public virtual async Task<CustomWebResponse> GetListAsync(ODataQueryOptions<TE> queryOptions, CancellationToken ct = default)
    {
        var query = entityRepository.GetQueryable();
        return await GetOdataListByQueryAsync(query, queryOptions, ct);
    }

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
            query = AddOdataQueryFilterAndOrder(query, queryOptions, DefaultOdataQuerySettings);

            var totalItems = await entityRepository.QueryCountAsync(query, ct);

            query = AddOdataQueryPagination(query, queryOptions, DefaultOdataQuerySettings);

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
    /// <param name="query">Linq Query.</param>
    /// <param name="queryOptions">OData query options.</param>
    /// <param name="settings">OData query settings.</param>
    /// <returns>Modified Linq query.</returns>
    private protected static IQueryable<TE> AddOdataQueryFilterAndOrder(IQueryable<TE> query, ODataQueryOptions<TE> queryOptions, ODataQuerySettings settings)
    {
        // Apply order and filter settings
        if (queryOptions?.Filter != null)
        {
            query = (IQueryable<TE>)queryOptions.Filter.ApplyTo(query, settings);
        }

        if (queryOptions.OrderBy != null)
        {
            query = queryOptions.OrderBy.ApplyTo(query, settings);
        }

        return query;
    }

    /// <summary>
    /// Add pagination settings to OData query.
    /// </summary>
    /// <param name="query">Linq Query.</param>
    /// <param name="queryOptions">OData query options.</param>
    /// <param name="settings">OData query settings.</param>
    /// <returns>Modified Linq query.</returns>
    private protected static IQueryable<TE> AddOdataQueryPagination(IQueryable<TE> query, ODataQueryOptions<TE> queryOptions, ODataQuerySettings settings)
    {
        const int maxPageSize = 20;

        if (queryOptions.Skip != null)
        {
            query = queryOptions.Skip.ApplyTo(query, settings);
        }

        var pageSize = queryOptions.Top?.Value ?? maxPageSize;
        if (pageSize > maxPageSize)
        {
            pageSize = maxPageSize;
        }

        query = query.Take(pageSize);

        return query;
    }
}
