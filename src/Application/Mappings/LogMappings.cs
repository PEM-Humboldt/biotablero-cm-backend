namespace IAVH.BioTablero.CM.Application.Mappings;

using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.DTOs.LogNS;
using IAVH.BioTablero.CM.Core.Entities.LogNS;

public class LogMappings : IMapper<LogEntity, LogDto>
{
    public LogDto Map(LogEntity entity) => new()
    {
        Id = entity.Id,
        Level = entity.Level,
        Type = entity.Type,
        TimeStamp = entity.TimeStamp,
        UserName = entity.UserName,
        CustomRecord = entity.CustomRecord,
        Message = entity.Message,
        ClientIp = entity.ClientIp,
        ClientAgent = entity.ClientAgent,
        Properties = entity.Properties,
    };

    public LogEntity Map(LogDto entity) => new()
    {
        Id = entity.Id,
        Level = entity.Level,
        Type = entity.Type,
        TimeStamp = entity.TimeStamp,
        UserName = entity.UserName,
        CustomRecord = entity.CustomRecord,
        Message = entity.Message,
        ClientIp = entity.ClientIp,
        ClientAgent = entity.ClientAgent,
        Properties = entity.Properties,
    };
}
