namespace IAVH.BioTablero.CM.Application.Mappings.Logging;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Logging;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Core.Domain.Entities.Logging;

/// <summary>
/// System logs mappings.
/// </summary>
public class LogMappings : IMapperRead<LogEntity, LogDto>
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
            TimeStamp = entity.TimeStamp.ToUniversalTime(),
            UserName = entity.UserName,
            CustomRecord = entity.CustomRecord,
            Message = entity.Message,
            ShortMessage = entity.ShortMessage,
            ClientIp = entity.ClientIp,
            ClientAgent = entity.ClientAgent,
            Properties = entity.Properties,
        };
    }
}
