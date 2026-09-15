namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Observation;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;

using IAVH.BioTablero.CM.WebApi.Interfaces;

/// <summary>
/// Observation Version edit response example.
/// </summary>
public class ObservationVersionEditRequestExample : IOpenApiExampleProvider<IndicatorVersionDto>
{
    /// <inheritdoc/>
    public IndicatorVersionDto GetExamples() => new()
    {
        Description = "Description example (edited)",
        Methodology = "Methodology example (edited)",
        Interpretation = "Interpretation example (edited)",
        Considerations = "Considerations example (edited)",
        Authorship = "Authorship example (edited)",
    };
}
