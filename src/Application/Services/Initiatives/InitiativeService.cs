namespace IAVH.BioTablero.CM.Application.Services.Initiatives;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Specifications;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

/// <summary>
/// Initiative service
/// </summary>
public class InitiativeService(IRepository<Initiative> entityRepository,
    IMapper<Initiative, InitiativeDto> mapper) : ServiceRead<Initiative, InitiativeDto, int, InitiativeSpec>(entityRepository, mapper), IInitiativeService
{
}
