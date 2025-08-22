namespace IAVH.BioTablero.CM.Application.Mappings;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Utils;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using InitiativeJoinRequestStatusEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.InitiativeJoinRequestStatus;

/// <summary>
/// Initiative Join Request mappings.
/// </summary>
public class InitiativeJoinRequestMappings() : IMapper<InitiativeJoinRequest, InitiativeJoinRequestDto>
{
    /// <summary>
    /// Map from entity to DTO.
    /// </summary>
    /// <param name="entity">Entity data.</param>
    /// <returns>DTO data.</returns>
    public InitiativeJoinRequestDto Map(InitiativeJoinRequest entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            UserName = entity.UserName,
            ReviewerUserName = entity.ReviewerUserName,
            CreationDate = entity.CreationDate,
            ResponseDate = entity.ResponseDate,
            InitiativeId = entity.InitiativeId,
            Status = new EnumEntityDto<InitiativeJoinRequestStatusEnum>(entity.StatusId),
        };
    }

    /// <summary>
    /// Map from DTO to entity.
    /// </summary>
    /// <param name="dto">DTO data.</param>
    /// <returns>Entity data.</returns>
    public InitiativeJoinRequest Map(InitiativeJoinRequestDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            UserName = dto.UserName,
            ReviewerUserName = dto.ReviewerUserName,
            CreationDate = dto.CreationDate,
            ResponseDate = dto.ResponseDate,
            InitiativeId = dto.InitiativeId,
            StatusId = dto.Status.Id,
        };
    }
}
