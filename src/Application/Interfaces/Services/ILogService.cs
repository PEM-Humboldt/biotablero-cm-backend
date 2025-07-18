namespace IAVH.BioTablero.CM.Application.Interfaces.Services;

using System;

using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.DTOs.LogNS;
using IAVH.BioTablero.CM.Core.Entities.LogNS;

public interface ILogService : IServiceRead<LogEntity, LogDto, Guid>
{
}
