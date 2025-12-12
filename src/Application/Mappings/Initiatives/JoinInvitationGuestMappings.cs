namespace IAVH.BioTablero.CM.Application.Mappings.Initiatives;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Join invitation guest mappings.
/// </summary>
public class JoinInvitationGuestMappings : IMapper<JoinInvitationGuest, JoinInvitationGuestDto>
{
    /// <summary>
    /// Map from entity to DTO.
    /// </summary>
    /// <param name="entity">Entity data.</param>
    /// <returns>DTO data.</returns>
    public JoinInvitationGuestDto Map(JoinInvitationGuest entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            JoinInvitationId = entity.JoinInvitationId,
            Email = entity.Email,
        };
    }

    /// <summary>
    /// Map from DTO to entity.
    /// </summary>
    /// <param name="dto">DTO data.</param>
    /// <returns>Entity data.</returns>
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
