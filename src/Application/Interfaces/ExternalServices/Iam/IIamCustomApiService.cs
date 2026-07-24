namespace IAVH.BioTablero.CM.Application.Interfaces.ExternalServices.Iam;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Core.Domain.Models.Iam;

/// <summary>
/// Identity and Access Management service interface for custom API.
/// </summary>
public interface IIamCustomApiService
{
    /// <summary>
    /// Get users data.
    /// </summary>
    /// <param name="query">OData query.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Users data.</returns>
    Task<List<ExternalUser>> GetUsersDataAsync(string query, CancellationToken ct = default);
}
