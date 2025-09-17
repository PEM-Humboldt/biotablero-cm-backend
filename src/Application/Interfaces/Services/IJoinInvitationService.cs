namespace IAVH.BioTablero.CM.Application.Interfaces.Services;

using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Join invitation service interface.
/// </summary>
public interface IJoinInvitationService : IRead<JoinInvitation, JoinInvitationDto, int>
{
    /// <summary>
    /// Add element.
    /// </summary>
    /// <param name="userName">Creator user name.</param>
    /// <param name="entityData">Entity data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Process result.</returns>
    Task<CustomWebResponse> AddAsync(string userName, JoinInvitationDto entityData, CancellationToken ct = default);
}
