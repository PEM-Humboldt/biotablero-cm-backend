namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Observation;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;

using IAVH.BioTablero.CM.WebApi.Interfaces;

/// <summary>
/// Observation Version response example.
/// </summary>
public class ObservationVersionResponseExample : IOpenApiExampleProvider<ObservationVersionDto>
{
    /// <inheritdoc/>
    public ObservationVersionDto GetExamples() => new()
    {
        Id = 0,
        ObservationId = 0,
        Version = 1,
        CreationDate = DateTime.Now,
        Description = "Description example",
        Methodology = "Methodology example",
        Interpretation = "Interpretation example",
        Considerations = "Considerations example",
        Authorship = "Authorship example",
    };
}
