namespace IAVH.BioTablero.CM.Application.Interfaces.General;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Domain;

/// <summary>
/// Delete data interface (for services).
/// </summary>
/// <typeparam name="TEntityType">Entity identifier type.</typeparam>
public interface IDelete<TEntityType>
    where TEntityType : notnull
{
    /// <summary>
    /// Delete element.
    /// </summary>
    /// <param name="id">Element identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> DeleteAsync(TEntityType id, CancellationToken ct = default);
}
