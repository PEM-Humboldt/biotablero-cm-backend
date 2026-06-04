namespace IAVH.BioTablero.CM.Application.DTOs.Indicators;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Measure Unit dto.
/// </summary>
public class MeasureUnitDto : IDto
{
    /// <summary>
    /// Measure Unit name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Measure Unit representation.
    /// </summary>
    public string Representation { get; set; }
}
