namespace IAVH.BioTablero.CM.Application.Interfaces.Services;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Initiative service interface
/// </summary>
public interface IInitiativeService : IServiceRead<Initiative, InitiativeDto, int>, IServiceAdd<InitiativeDto>, IServiceUpdate<InitiativeDto, int>, IServiceDisable<int>
{
}
