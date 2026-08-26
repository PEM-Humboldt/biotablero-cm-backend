namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Indicator;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;

using IAVH.BioTablero.CM.WebApi.Interfaces;

/// <summary>
/// Indicator Version response example.
/// </summary>
public class IndicatorVersionResponseExample : IOpenApiExampleProvider<IndicatorVersionDto>
{
    /// <inheritdoc/>
    public IndicatorVersionDto GetExamples() => new()
    {
        Id = 0,
        IndicatorId = 0,
        Version = 1,
        CreationDate = DateTime.Now,
        Description = "Description example",
        Methodology = "Methodology example",
        Interpretation = "Interpretation example",
        Considerations = "Considerations example",
        Authorship = "Authorship example",
    };
}
