namespace IAVH.BioTablero.CM.Core.Interfaces.ExternalServices;

using System.Threading;
using System.Threading.Tasks;

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
}
