namespace IAVH.BioTablero.CM.Application.Mappings;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Utils;
using IAVH.BioTablero.CM.Application.Interfaces.General;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using InitiativeUserLevelEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.InitiativeUserLevel;

/// <summary>
/// Initiative contact mappings
/// </summary>
public class InitiativeUserMappings : IMapper<InitiativeUser, InitiativeUserDto>
{
    /// <summary>
    /// Map from entity to DTO
    /// </summary>
    /// <param name="entity">Entity data</param>
    /// <returns>DTO data</returns>
    public InitiativeUserDto Map(InitiativeUser entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            InitiativeId = entity.InitiativeId,
            UserId = entity.UserId,
            Level = new EnumEntityDto<InitiativeUserLevelEnum>(entity.LevelId),
        };
    }

    /// <summary>
    /// Map from DTO to entity
    /// </summary>
    /// <param name="dto">DTO data</param>
    /// <returns>Entity data</returns>
    public InitiativeUser Map(InitiativeUserDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            Id = dto.Id,
            InitiativeId = dto.InitiativeId,
            UserId = dto.UserId,
            LevelId = dto.Level.Id,
        };
    }
}
