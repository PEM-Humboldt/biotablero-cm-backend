namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Topic;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;

using IAVH.BioTablero.CM.WebApi.Interfaces;

/// <summary>
/// Topic Version edit response example.
/// </summary>
public class TopicVersionEditRequestExample : IOpenApiExampleProvider<TopicVersionDto>
{
    /// <inheritdoc/>
    public TopicVersionDto GetExamples() => new()
    {
        Description = "Description example (edited)",
        Methodology = "Methodology example (edited)",
        Interpretation = "Interpretation example (edited)",
        Considerations = "Considerations example (edited)",
        Authorship = "Authorship example (edited)",
    };
}
