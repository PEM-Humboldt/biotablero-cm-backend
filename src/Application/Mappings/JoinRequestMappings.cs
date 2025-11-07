namespace IAVH.BioTablero.CM.Application.Mappings;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Utils;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using InitiativeUserLevelEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.InitiativeUserLevel;
using JoinRequestStatusEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.JoinRequestStatus;

/// <summary>
/// Join Request mappings.
/// </summary>
public class JoinRequestMappings() : IMapper<JoinRequest, JoinRequestDto>
{
    /// <summary>
    /// Map from entity to DTO.
    /// </summary>
    /// <param name="entity">Entity data.</param>
    /// <returns>DTO data.</returns>
    public JoinRequestDto Map(JoinRequest entity)
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
            Level = new EnumEntityDto<InitiativeUserLevelEnum>(entity.LevelId),
            Status = new EnumEntityDto<JoinRequestStatusEnum>(entity.StatusId),
        };
    }

    /// <summary>
    /// Map from DTO to entity.
    /// </summary>
    /// <param name="dto">DTO data.</param>
    /// <returns>Entity data.</returns>
    public JoinRequest Map(JoinRequestDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            UserName = dto.UserName,
            ReviewerUserName = dto.ReviewerUserName,
            CreationDate = dto.CreationDate,
            ResponseDate = dto.ResponseDate,
            InitiativeId = dto.InitiativeId,
            LevelId = dto.Level.Id,
            StatusId = dto.Status.Id,
        };
    }
}
