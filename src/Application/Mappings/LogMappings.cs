using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.DTOs.LogNS;
using IAVH.BioTablero.CM.Core.Entities.LogNS;

namespace IAVH.BioTablero.CM.Application.Mappings;

public class LogMappings : IMapper<LogEntity, LogDto>
{
    public LogDto Map(LogEntity entity) => new()
    {
        Id = entity.Id,
        Level = entity.Level,
        UtcTimeStamp = entity.UtcTimeStamp,
        RenderedMessage = entity.RenderedMessage,
        Properties = entity.Properties,
    };
    public LogEntity Map(LogDto entity) => new()
    {
        Id = entity.Id,
        Level = entity.Level,
        UtcTimeStamp = entity.UtcTimeStamp,
        RenderedMessage = entity.RenderedMessage,
        Properties = entity.Properties,
    };
}
