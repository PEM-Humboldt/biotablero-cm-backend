namespace IAVH.BioTablero.CM.Application.Interfaces.General;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Utils;
using IAVH.BioTablero.CM.Core.Interfaces.DTOs;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

using Microsoft.AspNetCore.OData.Query;

/// <summary>
/// Read data service interface
/// </summary>
/// <typeparam name="TE">Entity type</typeparam>
/// <typeparam name="TDto">DTO class type</typeparam>
/// <typeparam name="TI">Entity identifier type</typeparam>
public interface IServiceRead<TE, TDto, TI>
    where TE : class, IAggregateRoot
    where TDto : class, IDto
    where TI : notnull
{
    /// <summary>
    /// Check if element exists
    /// </summary>
    /// <param name="id">Element identifier</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    Task<bool> Exists(TI id, CancellationToken ct = default);

    /// <summary>
    /// Get element
    /// </summary>
    /// <param name="id">Element identifier</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    Task<CustomWebResponse> GetItem(TI id, CancellationToken ct = default);

    /// <summary>
    /// Get all elements
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    Task<CustomWebResponse> GetAll(CancellationToken ct = default);

    /// <summary>
    /// Get elements list (paginated)
    /// </summary>
    /// <param name="skip">Page</param>
    /// <param name="take">Page size</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    Task<CustomWebResponse> GetList(int skip, int take, CancellationToken ct = default);

    /// <summary>
    /// Get elements list (OData)
    /// </summary>
    /// <param name="queryOptions">OData query options</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    Task<CustomWebResponse> GetList(ODataQueryOptions<TE> queryOptions, CancellationToken ct = default);
}
