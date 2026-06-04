namespace IAVH.BioTablero.CM.Application.DTOs.Indicators;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Category dto.
/// </summary>
public class CategoryDto : IDto
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Category name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Category description.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Parent relationship.
    /// </summary>
    public CategoryDto Parent { get; set; }
}
