namespace IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Initiative User entity
/// </summary>
public class InitiativeUserDto : IDto
{
    /// <summary>
    /// Item identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Initiative identifier
    /// </summary>
    public int InitiativeId { get; set; }

    /// <summary>
    /// User identifier
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Level identifier
    /// </summary>
    public int LevelId { get; set; }

    /// <summary>
    /// Level relationship
    /// </summary>
    public InitiativeUserLevelDto Level { get; set; }
}
