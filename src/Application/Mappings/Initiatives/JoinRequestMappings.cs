namespace IAVH.BioTablero.CM.Application.Mappings.Initiatives;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Utils;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using InitiativeUserLevelEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.InitiativeUserLevel;
using JoinRequestStatusEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.JoinRequestStatus;

/// <summary>
/// Join Request mappings.
/// </summary>
public class JoinRequestMappings() : IMapperCreateAndRead<JoinRequest, JoinRequestDto>
{
    /// <inheritdoc/>
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
            Level = entity.LevelId != null ? new EnumEntityDto<InitiativeUserLevelEnum>(entity.LevelId.Value) : null,
            Status = new EnumEntityDto<JoinRequestStatusEnum>(entity.StatusId),
        };
    }

    /// <inheritdoc/>
    public JoinRequest Map(JoinRequestDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            UserName = dto.UserName,
            ReviewerUserName = dto.ReviewerUserName,
            CreationDate = dto.CreationDate ?? DateTime.Now,
            ResponseDate = dto.ResponseDate,
            InitiativeId = dto.InitiativeId,
            LevelId = dto.Level?.Id,
            StatusId = dto.Status.Id,
        };
    }
}
