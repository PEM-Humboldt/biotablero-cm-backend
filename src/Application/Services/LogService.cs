namespace IAVH.BioTablero.CM.Application.Services;

using IAVH.BioTablero.CM.Application.Interfaces.Services;
using IAVH.BioTablero.CM.Application.Specifications;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.DTOs.LogNS;
using IAVH.BioTablero.CM.Core.Entities.LogNS;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OData.Query;
using System.Threading;
using System.Linq;
using IAVH.BioTablero.CM.Core.Helpers.General;
using System.Collections.Generic;

public class LogService(IRepository<LogEntity> entityRepository,
    IMapper<LogEntity, LogDto> mapper) : ServiceRead<LogEntity, LogDto, Guid, LogSpec>(entityRepository, mapper), ILogService
{
}