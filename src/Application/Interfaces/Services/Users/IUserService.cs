namespace IAVH.BioTablero.CM.Application.Interfaces.Services.Users;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Utils;

/// <summary>
/// User service interface.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Get element.
    /// </summary>
    /// <param name="userName">User name.</param>
    /// <param name="roles">User roles.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> GetAllAsync(string userName, string[] roles, CancellationToken ct = default);
}
