namespace IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using IAVH.BioTablero.CM.Application.DTOs.Utils;
using IAVH.BioTablero.CM.Application.Interfaces.General;

using static IAVH.BioTablero.CM.Core.Domain.Utils.Enums.InitiativesEnums;

/// <summary>
/// Initiative User entity
/// </summary>
public class InitiativeUserDto : IDto
{
    /// <summary>
    /// Item identifier
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Initiative identifier
    /// </summary>
    public int? InitiativeId { get; set; }

    /// <summary>
    /// User identifier
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Level relationship
    /// </summary>
    public EnumEntityDto<InitiativeUserLevel> Level { get; set; }
}
