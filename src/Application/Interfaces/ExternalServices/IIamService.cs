namespace IAVH.BioTablero.CM.Application.Interfaces.ExternalServices;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Utils.Iam;

/// <summary>
/// Identity and Access Management service interface.
/// </summary>
public interface IIamService
{
    /// <summary>
    /// Check if user exists.
    /// </summary>
    /// <param name="username">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if user exists. False otherwise.</returns>
    Task<bool> UserExistsAsync(string username, CancellationToken ct = default);

    /// <summary>
    /// Get user data.
    /// </summary>
    /// <param name="username">User name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>User data.</returns>
    Task<ExternalUser> GetUserDataAsync(string username, CancellationToken ct = default);

    /// <summary>
    /// Get users data.
    /// </summary>
    /// <param name="usernames">User name list.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Users data.</returns>
    Task<IEnumerable<ExternalUser>> GetUsersDataAsync(string[] usernames, CancellationToken ct = default);
}
