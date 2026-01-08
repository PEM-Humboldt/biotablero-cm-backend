namespace IAVH.BioTablero.CM.Application.Mappings.Initiatives;

using System;
using System.Linq;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Join invitation mappings.
/// </summary>
public class JoinInvitationMappings(
    IMapper<JoinInvitationGuest, JoinInvitationGuestDto> joinInvitationGuestMappings) : IMapper<JoinInvitation, JoinInvitationDto>
{
    /// <inheritdoc/>
    public JoinInvitationDto Map(JoinInvitation entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            InitiativeId = entity.InitiativeId,
            Creator = entity.Creator,
            Message = entity.Message,
            CreationDate = entity.CreationDate,
            Guests = entity.Guests?.Select(joinInvitationGuestMappings.Map),
        };
    }

    /// <inheritdoc/>
    public JoinInvitation Map(JoinInvitationDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            InitiativeId = dto.InitiativeId,
            Creator = dto.Creator,
            Message = dto.Message,
            CreationDate = dto.CreationDate ?? DateTime.Now,
            Guests = [.. dto.Guests?.Select(joinInvitationGuestMappings.Map)],
        };
    }
}
