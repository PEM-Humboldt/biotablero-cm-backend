namespace IAVH.BioTablero.CM.Application.Interfaces.Services;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Join invitation service interface.
/// </summary>
public interface IJoinInvitationService : IRead<JoinInvitation, int>, IAdd<JoinInvitationDto>
{
}
