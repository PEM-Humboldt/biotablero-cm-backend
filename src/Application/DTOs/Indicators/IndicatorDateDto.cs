namespace IAVH.BioTablero.CM.Application.DTOs.Indicators;

using System;

using IAVH.BioTablero.CM.Application.Interfaces.General;

/// <summary>
/// Indicator Date dto.
/// </summary>
/// <param name="date">Date.</param>
public class IndicatorDateDto(DateTimeOffset date) : IDto
{
    /// <summary>
    /// Date year.
    /// </summary>
    public int Year { get; set; } = date.Year;

    /// <summary>
    /// Date month.
    /// </summary>
    public int Month { get; set; } = date.Month;
}
