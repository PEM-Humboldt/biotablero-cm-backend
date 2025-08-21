namespace IAVH.BioTablero.CM.Application.Interfaces.Services;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Initiative Tag service interface.
/// </summary>
public interface IInitiativeTagService : IServiceRead<InitiativeTag, InitiativeTagDto, int>, IServiceAdd<InitiativeTagDto>, IServiceUpdate<InitiativeTagDto, int>, IServiceDelete<int>
{
}
