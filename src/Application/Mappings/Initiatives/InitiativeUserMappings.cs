namespace IAVH.BioTablero.CM.Application.Mappings.Initiatives;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Initiatives;
using IAVH.BioTablero.CM.Application.DTOs.Utils;
using IAVH.BioTablero.CM.Application.Interfaces.General.Mapper;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;

using InitiativeUserLevelEnum = IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums.InitiativeUserLevel;

/// <summary>
/// Initiative contact mappings.
/// </summary>
public class InitiativeUserMappings : IMapperCreateReadAndUpdate<InitiativeUser, InitiativeUserDto>
{
    /// <inheritdoc/>
    public InitiativeUserDto Map(InitiativeUser entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new()
        {
            Id = entity.Id,
            InitiativeId = entity.InitiativeId,
            UserName = entity.UserName,
            FocusArea = entity.FocusArea,
            Level = new EnumEntityDto<InitiativeUserLevelEnum>(entity.LevelId),
            CreationDate = entity.CreationDate,
        };
    }

    /// <inheritdoc/>
    public InitiativeUser Map(InitiativeUserDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            InitiativeId = dto?.InitiativeId ?? 0,
            UserName = dto.UserName,
            FocusArea = dto.FocusArea,
            LevelId = dto.Level.Id,
            CreationDate = dto.CreationDate ?? DateTime.Now,
        };
    }

    /// <inheritdoc/>
    public void Update(InitiativeUser entity, InitiativeUserDto dto)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(dto);

        entity.LevelId = dto.Level.Id;
        entity.FocusArea = dto.FocusArea;
    }
}
