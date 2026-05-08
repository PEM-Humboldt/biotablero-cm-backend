namespace IAVH.BioTablero.CM.Application.Mappings.Initiatives;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Mappings.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Join invitation guest mappings.
/// </summary>
public class JoinInvitationGuestMappings : MapperRead<JoinInvitationGuest, JoinInvitationGuestDto>, IMapperCreateAndRead<JoinInvitationGuest, JoinInvitationGuestDto>
{
    /// <inheritdoc/>
    public override JoinInvitationGuestDto Map(JoinInvitationGuest entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            JoinInvitationId = entity.JoinInvitationId,
            Email = entity.Email,
        };
    }

    /// <inheritdoc/>
    public JoinInvitationGuest Map(JoinInvitationGuestDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            JoinInvitationId = dto.JoinInvitationId ?? 0,
            Email = dto.Email,
        };
    }
}
