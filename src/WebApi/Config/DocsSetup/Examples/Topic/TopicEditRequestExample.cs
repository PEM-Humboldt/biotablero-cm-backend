namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Topic;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;

using IAVH.BioTablero.CM.WebApi.Interfaces;

/// <summary>
/// Topic edit response example.
/// </summary>
public class TopicEditRequestExample : IOpenApiExampleProvider<TopicDto>
{
    /// <inheritdoc/>
    public TopicDto GetExamples() => new()
    {
        Name = "Topic example (edited)",
    };
}
