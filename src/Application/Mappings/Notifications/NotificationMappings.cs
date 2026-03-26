namespace IAVH.BioTablero.CM.Application.Mappings.Notifications;

using System;
using System.Collections.Generic;
using System.Text.Json;

using IAVH.BioTablero.CM.Application.DTOs.Notifications;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Core.Domain.Entities.Notifications;

/// <summary>
/// Notification mappings.
/// </summary>
public class NotificationMappings : IMapperCreateAndRead<Notification, NotificationDto>
{
    /// <inheritdoc/>
    public NotificationDto Map(Notification entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            Receiver = entity.Receiver,
            Subject = entity.Subject,
            Body = entity.Body,
            CreationDate = entity.CreationDate,
            ReadingDate = entity.ReadingDate,
            Readed = entity.Readed,
            Properties = JsonSerializer.Deserialize<Dictionary<string, object>>(entity.Properties),
        };
    }

    /// <inheritdoc/>
    public Notification Map(NotificationDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            Id = dto.Id,
            Receiver = dto.Receiver,
            Subject = dto.Subject,
            Body = dto.Body,
            CreationDate = dto.CreationDate,
            ReadingDate = dto.ReadingDate,
            Readed = dto.Readed,
            Properties = JsonSerializer.Serialize(dto.Properties),
        };
    }
}
