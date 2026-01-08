namespace IAVH.BioTablero.CM.Application.Mappings.Logging;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Logging;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Logging;

/// <summary>
/// System logs mappings.
/// </summary>
public class LogMappings : IMapper<LogEntity, LogDto>
{
    /// <inheritdoc/>
    public LogDto Map(LogEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            Level = entity.Level,
            Type = entity.Type,
            TimeStamp = entity.TimeStamp,
            UserName = entity.UserName,
            CustomRecord = entity.CustomRecord,
            Message = entity.Message,
            ShortMessage = entity.ShortMessage,
            ClientIp = entity.ClientIp,
            ClientAgent = entity.ClientAgent,
            Properties = entity.Properties,
        };
    }

    /// <inheritdoc/>
    public LogEntity Map(LogDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            Id = dto.Id,
            Level = dto.Level,
            Type = dto.Type,
            TimeStamp = dto.TimeStamp,
            UserName = dto.UserName,
            CustomRecord = dto.CustomRecord,
            Message = dto.Message,
            ShortMessage = dto.ShortMessage,
            ClientIp = dto.ClientIp,
            ClientAgent = dto.ClientAgent,
            Properties = dto.Properties,
        };
    }
}
