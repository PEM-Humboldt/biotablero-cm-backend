namespace IAVH.BioTablero.CM.Application.Mappings.Logging;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Logging;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Logging;

/// <summary>
/// System logs mappings for OData.
/// </summary>
public class LogBaseMappings : IMapper<LogEntity, LogBaseDto>
{
    /// <summary>
    /// Map from entity to DTO.
    /// </summary>
    /// <param name="entity">Entity data.</param>
    /// <returns>DTO data.</returns>
    public LogBaseDto Map(LogEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            Type = entity.Type,
            TimeStamp = entity.TimeStamp,
            UserName = entity.UserName,
            ShortMessage = entity.ShortMessage,
        };
    }

    /// <summary>
    /// Map from DTO to entity.
    /// </summary>
    /// <param name="dto">DTO data.</param>
    /// <returns>Entity data.</returns>
    public LogEntity Map(LogBaseDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            Id = dto.Id,
            Type = dto.Type,
            TimeStamp = dto.TimeStamp,
            UserName = dto.UserName,
            ShortMessage = dto.ShortMessage,
        };
    }
}
