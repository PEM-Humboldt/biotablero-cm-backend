namespace IAVH.BioTablero.CM.Application.Mappings;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Logging;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Logging;

/// <summary>
/// System logs mappings
/// </summary>
public class LogMappings : IMapper<LogEntity, LogDto>
{
    /// <summary>
    /// Map from entity to DTO
    /// </summary>
    /// <param name="entity">Entity data</param>
    /// <returns>DTO data</returns>
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
            ClientIp = entity.ClientIp,
            ClientAgent = entity.ClientAgent,
            Properties = entity.Properties,
        };
    }

    /// <summary>
    /// Map from DTO to entity
    /// </summary>
    /// <param name="dto">DTO data</param>
    /// <returns>Entity data</returns>
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
            ClientIp = dto.ClientIp,
            ClientAgent = dto.ClientAgent,
            Properties = dto.Properties,
        };
    }
}
