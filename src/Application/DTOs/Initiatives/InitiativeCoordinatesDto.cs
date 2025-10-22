namespace IAVH.BioTablero.CM.Application.DTOs.Initiatives;

using System;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Initiative coordinates data transfer object.
/// </summary>
public class InitiativeCoordinatesDto : IDto
{
    /// <summary>
    /// Initiative identifier.
    /// </summary>
    public int InitiativeId { get; set; }

    /// <summary>
    /// Initiative name.
    /// </summary>
    public string InitiativeName { get; set; } = string.Empty;

    /// <summary>
    /// Initiative coordinates [longitude, latitude].
    /// </summary>
    public double[] Coordinate { get; set; } = Array.Empty<double>();
}
