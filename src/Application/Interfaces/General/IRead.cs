namespace IAVH.BioTablero.CM.Application.Interfaces.General;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Domain;
using IAVH.BioTablero.CM.Core.Interfaces.Entities;

using Microsoft.AspNetCore.OData.Query;

/// <summary>
/// Read data interface (for services).
/// </summary>
/// <typeparam name="TE">Entity type.</typeparam>
/// <typeparam name="TI">Entity identifier type.</typeparam>
public interface IRead<TE, TI>
    where TE : class, IAggregateRoot
    where TI : notnull
{
    /// <summary>
    /// Check if element exists.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<bool> AnyAsync(TI id, CancellationToken ct = default);

    /// <summary>
    /// Get element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> GetItemAsync(TI id, CancellationToken ct = default);

    /// <summary>
    /// Get all elements.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> GetAllAsync(CancellationToken ct = default);

    /// <summary>
    /// Get elements list (OData).
    /// </summary>
    /// <param name="queryOptions">OData query options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> GetListAsync(ODataQueryOptions<TE> queryOptions, CancellationToken ct = default);
}
