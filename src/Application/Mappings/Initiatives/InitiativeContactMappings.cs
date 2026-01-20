namespace IAVH.BioTablero.CM.Application.Mappings.Initiatives;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Initiative contact mappings.
/// </summary>
public class InitiativeContactMappings : IMapper<InitiativeContact, InitiativeContactDto>
{
    /// <inheritdoc/>
    public InitiativeContactDto Map(InitiativeContact entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            InitiativeId = entity.InitiativeId,
            Phone = entity.Phone,
            Email = entity.Email,
        };
    }

    /// <inheritdoc/>
    public InitiativeContact Map(InitiativeContactDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            InitiativeId = dto?.InitiativeId ?? 0,
            Phone = dto.Phone,
            Email = dto.Email,
        };
    }
}
