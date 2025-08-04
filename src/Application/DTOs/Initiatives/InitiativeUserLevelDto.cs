namespace IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Level entity
/// </summary>
public class InitiativeUserLevelDto : IDto
{
    /// <summary>
    /// Item identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Level name
    /// </summary>
    public string Name { get; set; }
}
