namespace IAVH.BioTablero.CM.Application.Mappings.Initiatives;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Mappings.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Monitoring Events mappings.
/// </summary>
public class MonitoringEventsMappings : MapperRead<MonitoringEvents, MonitoringEventsDto>, IMapperCreateReadAndUpdate<MonitoringEvents, MonitoringEventsDto>
{
    /// <inheritdoc/>
    public override MonitoringEventsDto Map(MonitoringEvents? entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            InitiativeId = entity.InitiativeId,
            Date = entity.Date.ToUniversalTime(),
            Value = entity.Value,
        };
    }

    /// <inheritdoc/>
    public MonitoringEvents Map(MonitoringEventsDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            Id = dto.Id ?? 0,
            InitiativeId = dto.InitiativeId,
            Date = dto.Date,
            Value = dto.Value,
        };
    }

    /// <inheritdoc/>
    public void Update(MonitoringEvents entity, MonitoringEventsDto dto)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(dto);

        entity.Date = dto.Date;
        entity.Value = dto.Value;
    }
}
