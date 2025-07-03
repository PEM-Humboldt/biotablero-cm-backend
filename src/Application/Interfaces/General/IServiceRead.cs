namespace IAVH.BioTablero.CM.Application.Interfaces.General;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Helpers.General;
using IAVH.BioTablero.CM.Core.interfaces;

/// <summary>
/// Read data service interface 
/// </summary>
/// <typeparam name="DTO">DTO class type</typeparam>
/// <typeparam name="ET">Entity identifier type</typeparam>
public interface IServiceRead<DTO, ET>
    where DTO : class, IDto
    where ET : notnull
{
    /// <summary>
    /// Check if element exists
    /// </summary>
    /// <param name="id">Element identifier</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    public Task<bool> Exists(ET id, CancellationToken ct = default);

    /// <summary>
    /// Get element
    /// </summary>
    /// <param name="id">Element identifier</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    public Task<CustomWebResponse> Get(ET id, CancellationToken ct = default);

    /// <summary>
    /// Get all elements
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    public Task<CustomWebResponse> GetAll(CancellationToken ct = default);

    /// <summary>
    /// Get elements list (paginated)
    /// </summary>
    /// <param name="skip">Page</param>
    /// <param name="take">Page size</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    public Task<CustomWebResponse> GetList(int skip, int take, CancellationToken ct = default);
}