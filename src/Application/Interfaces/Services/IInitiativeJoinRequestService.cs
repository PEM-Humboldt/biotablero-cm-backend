namespace IAVH.BioTablero.CM.Application.Interfaces.Services;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Initiative Join request service interface.
/// </summary>
public interface IInitiativeJoinRequestService : IServiceRead<InitiativeJoinRequest, InitiativeJoinRequestDto, int>, IServiceAdd<InitiativeJoinRequestDto>, IServiceUpdate<InitiativeJoinRequestDto, int>
{
}
