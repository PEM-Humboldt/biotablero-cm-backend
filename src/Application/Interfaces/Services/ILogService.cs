namespace IAVH.BioTablero.CM.Application.Interfaces.Services;

using System;

using IAVH.BioTablero.CM.Application.DTOs.LogNS;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.LogNS;

/// <summary>
/// System logs service interface
/// </summary>
public interface ILogService : IServiceRead<LogEntity, LogDto, Guid>
{
}
