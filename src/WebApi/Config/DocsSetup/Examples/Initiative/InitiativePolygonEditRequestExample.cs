namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Initiative;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Initiative polygon edit response example.
/// </summary>
public class InitiativePolygonEditRequestExample : IExamplesProvider<string>
{
    /// <inheritdoc/>
    public string GetExamples() => "{\"coordinates\":[[[-74.1563522266587,4.662729197971359],[-74.09318638402624,4.568287702377319],[-74.03317883352514,4.704699136317785],[-74.1563522266587,4.662729197971359]]],\"type\":\"Polygon\"}";
}
