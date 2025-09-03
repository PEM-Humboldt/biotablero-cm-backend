namespace IAVH.BioTablero.CM.Application.Interfaces.General;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Utils;

/// <summary>
/// Add data interface (for services).
/// </summary>
/// <typeparam name="TDto">DTO class type.</typeparam>
public interface IAdd<TDto>
    where TDto : class, IDto
{
    /// <summary>
    /// Add element.
    /// </summary>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> AddAsync(TDto entityData, CancellationToken ct = default);
}
