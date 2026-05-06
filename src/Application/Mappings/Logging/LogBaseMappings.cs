namespace IAVH.BioTablero.CM.Application.Mappings.Logging;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Logging;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Core.Domain.Entities.Logging;

/// <summary>
/// System logs mappings for OData.
/// </summary>
public class LogBaseMappings : IMapperRead<LogEntity, LogBaseDto>
{
    /// <inheritdoc/>
    public LogBaseDto Map(LogEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            Type = entity.Type,
            TimeStamp = entity.TimeStamp.ToUniversalTime(),
            UserName = entity.UserName,
            ShortMessage = entity.ShortMessage,
        };
    }
}
