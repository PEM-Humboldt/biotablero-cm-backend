namespace IAVH.BioTablero.CM.Application.Interfaces.Services;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Initiative User service interface.
/// </summary>
public interface IInitiativeUserService : IServiceRead<InitiativeUser, InitiativeUserDto, int>, IServiceAdd<InitiativeUserDto>, IServiceUpdate<InitiativeUserDto, int>, IServiceDelete<int>
{
    /// <summary>
    /// Get entities by initiative.
    /// </summary>
    /// <param name="initiativeId">Initiative identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> GetByInitiative(int initiativeId, CancellationToken ct = default);
}
