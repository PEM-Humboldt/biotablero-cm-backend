namespace IAVH.BioTablero.CM.Application.Interfaces.General;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Utils;

/// <summary>
/// Update data interface (for services).
/// </summary>
/// <typeparam name="TDto">DTO class type.</typeparam>
/// <typeparam name="TEntityType">Entity identifier type.</typeparam>
public interface IUpdate<TDto, TEntityType>
    where TDto : class, IDto
    where TEntityType : notnull
{
    /// <summary>
    /// Update element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> UpdateAsync(TEntityType id, TDto entityData, CancellationToken ct = default);
}
