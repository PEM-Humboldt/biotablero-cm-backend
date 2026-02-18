namespace IAVH.BioTablero.CM.Application.Interfaces.Services.Initiatives;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Utils;

/// <summary>
/// Initiative Tag service interface.
/// </summary>
public interface IInitiativeTagService : IDelete<int>
{
    /// <summary>
    /// Add element.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="tagId">Tag identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> AddAsync(int initiativeId, int tagId, CancellationToken ct = default);
}
