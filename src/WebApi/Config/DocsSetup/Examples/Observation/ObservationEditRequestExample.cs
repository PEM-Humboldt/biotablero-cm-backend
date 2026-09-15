namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Observation;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;

using IAVH.BioTablero.CM.WebApi.Interfaces;

/// <summary>
/// Observation edit response example.
/// </summary>
public class ObservationEditRequestExample : IOpenApiExampleProvider<ObservationDto>
{
    /// <inheritdoc/>
    public ObservationDto GetExamples() => new()
    {
        Name = "Observation example (edited)",
    };
}
