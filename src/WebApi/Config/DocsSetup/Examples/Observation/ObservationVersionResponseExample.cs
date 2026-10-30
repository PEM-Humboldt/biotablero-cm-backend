namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Observation;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;

using IAVH.BioTablero.CM.WebApi.Interfaces;

/// <summary>
/// Observation Version response example.
/// </summary>
public class ObservationVersionResponseExample : IOpenApiExampleProvider<TopicVersionDto>
{
    /// <inheritdoc/>
    public TopicVersionDto GetExamples() => new()
    {
        Id = 0,
        TopicId = 0,
        Version = 1,
        CreationDate = DateTime.Now,
        Description = "Description example",
        Methodology = "Methodology example",
        Interpretation = "Interpretation example",
        Considerations = "Considerations example",
        Authorship = "Authorship example",
    };
}
