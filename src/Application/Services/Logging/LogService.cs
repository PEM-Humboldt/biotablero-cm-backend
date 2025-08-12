namespace IAVH.BioTablero.CM.Application.Services.Logging;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Logging;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Application.Services.General;
using IAVH.BioTablero.CM.Application.Specifications;
using IAVH.BioTablero.CM.Core.Domain.Entities.Logging;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

/// <summary>
/// System logs service
/// </summary>
public class LogService(IRepository<LogEntity> entityRepository,
    IMapper<LogEntity, LogDto> mapper) : ServiceRead<LogEntity, LogDto, Guid, LogSpec>(entityRepository, mapper), ILogService
{
}
