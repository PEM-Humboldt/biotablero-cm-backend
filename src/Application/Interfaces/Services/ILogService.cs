using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.DTOs.LogNS;

namespace IAVH.BioTablero.CM.Application.Interfaces.Services;

public interface ILogService : IServiceRead<LogDto, string>
{ }