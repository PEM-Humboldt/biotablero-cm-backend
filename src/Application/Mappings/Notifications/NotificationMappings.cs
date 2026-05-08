namespace IAVH.BioTablero.CM.Application.Mappings.Notifications;

using System;
using System.Text.Json;

using IAVH.BioTablero.CM.Application.DTOs.Notifications;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Mappings.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Notifications;

/// <summary>
/// Notification mappings.
/// </summary>
public class NotificationMappings : MapperRead<Notification, NotificationDto>, IMapperCreateAndRead<Notification, NotificationDto>
{
    /// <inheritdoc/>
    public override NotificationDto Map(Notification entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            Receiver = entity.Receiver,
            Subject = entity.Subject,
            Body = entity.Body,
            CreationDate = entity.CreationDate.ToUniversalTime(),
            ReadingDate = entity.ReadingDate?.ToUniversalTime(),
            IsRead = entity.IsRead,
            Properties = entity.Properties != null ? JsonSerializer.Deserialize<NotificationPropertiesDto>(entity.Properties) : null,
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
            IsRead = dto.IsRead,
            Properties = JsonSerializer.Serialize(dto.Properties),
        };
    }
}
