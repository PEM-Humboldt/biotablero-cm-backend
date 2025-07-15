namespace IAVH.BioTablero.CM.Application.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Specifications;
using IAVH.BioTablero.CM.Core.Helpers.General;
using IAVH.BioTablero.CM.Core.interfaces;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// General service for only read functions
/// </summary>
/// <typeparam name="E">Entity type</typeparam>
/// <typeparam name="DTO">DTO class type</typeparam>
/// <typeparam name="ET">Entity identifier type</typeparam>
/// <typeparam name="GS">General Specification type</typeparam>
/// <remarks>
/// Initialize service
/// </remarks>
public abstract class ServiceRead<E, DTO, ET, GS>(IRepository<E> entityRepository, IMapper<E, DTO> mapper) : IServiceRead<E, DTO, ET>
    where DTO : class, IDto
    where ET : notnull
    where E : class, IAggregateRoot
    where GS : GeneralSpecification<ET, E>
{
    private readonly IRepository<E> entityRepository = entityRepository;
    private readonly IMapper<E, DTO> mapper = mapper;

    /// <summary>
    /// Check if element exists
    /// </summary>
    /// <param name="id">Element identifier</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    public virtual async Task<bool> Exists(ET id, CancellationToken ct = default)
    {
        var specification = (GS)Activator.CreateInstance(typeof(GS), [id]);
        return await entityRepository.AnyAsync(specification, ct);
    }

    /// <summary>
    /// Get element
    /// </summary>
    /// <param name="id">Element identifier</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    public virtual async Task<CustomWebResponse> Get(ET id, CancellationToken ct = default)
    {
        var specification = (GS)Activator.CreateInstance(typeof(GS), [id]);
        var dataEntity = await entityRepository.FirstOrDefaultAsync(specification, ct);

        if (dataEntity != null)
        {
            var dataDto = mapper.Map(dataEntity);
            return new()
            {
                ResponseBody = dataDto
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
    /// Get all elements
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    public virtual async Task<CustomWebResponse> GetAll(CancellationToken ct = default)
    {
        var dataListEntity = await entityRepository.ListAsync(ct);
        var dataListDto = dataListEntity
            .Select(mapper.Map);

        return new()
        {
            ResponseBody = dataListDto
        };
    }

    /// <summary>
    /// Get elements list (paginated)
    /// </summary>
    /// <param name="skip">Page</param>
    /// <param name="take">Page size</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    public virtual async Task<CustomWebResponse> GetList(int skip, int take, CancellationToken ct = default)
    {
        var specification = (GS)Activator.CreateInstance(typeof(GS), [skip, take]);
        var dataListEntity = await entityRepository.ListAsync(specification, ct);
        var dataListDto = dataListEntity
            .Select(mapper.Map);

        return new()
        {
            ResponseBody = dataListDto
        };
    }

    /// <summary>
    /// Get elements list (OData)
    /// </summary>
    /// <param name="queryOptions">OData query options</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    public virtual async Task<CustomWebResponse> GetList(ODataQueryOptions<E> queryOptions, CancellationToken ct = default)
    {
        var query = entityRepository.GetQueryable();

        var settings = new ODataQuerySettings
        {
            HandleNullPropagation = HandleNullPropagationOption.True
        };

        // Apply order and filter settings
        if (queryOptions.Filter != null)
        {
            query = (IQueryable<E>)queryOptions.Filter.ApplyTo(query, settings);
        }

        if (queryOptions.OrderBy != null)
        {
            query = queryOptions.OrderBy.ApplyTo(query, settings);
        }

        // Check total items
        var totalCount = await query.CountAsync(ct);

        // Apply pagination settings ($skip and $top)
        if (queryOptions.Skip != null)
        {
            query = queryOptions.Skip.ApplyTo(query, settings);
        }

        if (queryOptions.Top != null)
        {
            query = queryOptions.Top.ApplyTo(query, settings);
        }

        // Get result
        var dataList = await query.ToListAsync(ct);

        return new()
        {
            ResponseBody = new Dictionary<string, object>
            {
                ["@odata.count"] = totalCount,
                ["value"] = dataList.Select(mapper.Map),
            }
        };
    }
}
