namespace IAVH.BioTablero.CM.Application.Mappings.Initiatives;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Mappings.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Initiative contact mappings.
/// </summary>
public class InitiativeContactMappings : MapperRead<InitiativeContact, InitiativeContactDto>, IMapperCreateReadAndUpdate<InitiativeContact, InitiativeContactDto>
{
    /// <inheritdoc/>
    public override InitiativeContactDto Map(InitiativeContact entity)
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

    /// <inheritdoc/>
    public void Update(InitiativeContact entity, InitiativeContactDto dto)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(dto);

        entity.Email = dto.Email;
        entity.Phone = dto.Phone;
    }
}
