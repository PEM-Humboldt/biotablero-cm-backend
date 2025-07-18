namespace IAVH.BioTablero.CM.Application.Services;

using System;

using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Application.Specifications;
using IAVH.BioTablero.CM.Core.DTOs.LogNS;
using IAVH.BioTablero.CM.Core.Entities.LogNS;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

public class LogService(IRepository<LogEntity> entityRepository,
    IMapper<LogEntity, LogDto> mapper) : ServiceRead<LogEntity, LogDto, Guid, LogSpec>(entityRepository, mapper), ILogService
{ }
