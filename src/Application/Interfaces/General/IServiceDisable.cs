namespace IAVH.BioTablero.CM.Application.Interfaces.General;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Utils;

/// <summary>
/// Disable service interface
/// </summary>
/// <typeparam name="TEntityId">Entity identifier type</typeparam>
public interface IServiceDisable<TEntityId>
    where TEntityId : notnull
{
    /// <summary>
    /// Disable or enable element
    /// </summary>
    /// <param name="id">Element identifier</param>
    /// <param name="disable">Disable flag</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    Task<CustomWebResponse> Disable(TEntityId id, bool disable, CancellationToken ct = default);
}
