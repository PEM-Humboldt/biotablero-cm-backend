namespace IAVH.BioTablero.CM.Application.Mappings.Initiatives;

using System;
using System.Linq;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Application.Mappings.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

/// <summary>
/// Join invitation mappings.
/// </summary>
public class JoinInvitationMappings(
    IMapperCreateAndRead<JoinInvitationGuest, JoinInvitationGuestDto> joinInvitationGuestMappings) : MapperRead<JoinInvitation, JoinInvitationDto>, IMapperCreateAndRead<JoinInvitation, JoinInvitationDto>
{
    /// <inheritdoc/>
    public override JoinInvitationDto Map(JoinInvitation entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            InitiativeId = entity.InitiativeId,
            Creator = entity.Creator,
            Message = entity.Message,
            CreationDate = entity.CreationDate.ToUniversalTime(),
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
            CreationDate = dto.CreationDate ?? DateTimeOffset.UtcNow,
            Guests = [.. dto.Guests?.Select(joinInvitationGuestMappings.Map)],
        };
    }
}
