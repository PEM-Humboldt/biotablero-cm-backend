using System;

using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.DTOs.LogNS;
using IAVH.BioTablero.CM.Core.Entities.LogNS;

namespace IAVH.BioTablero.CM.Application.Interfaces.Services;

public interface ILogService : IServiceRead<LogEntity, LogDto, Guid>
{ }
