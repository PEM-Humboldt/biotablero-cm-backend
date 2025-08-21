namespace IAVH.BioTablero.CM.Application.Interfaces.Services;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Utils;

/// <summary>
/// Initiative Tag Initiative service interface.
/// </summary>
public interface IInitiativeTagInitiativeService : IServiceDelete<int>
{
    /// <summary>
    /// Add element.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="tagId">Initiative tag identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> Add(int initiativeId, int tagId, CancellationToken ct = default);
}
