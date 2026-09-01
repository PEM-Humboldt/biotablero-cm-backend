namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Indicator;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Indicator Version edit response example.
/// </summary>
public class IndicatorVersionEditRequestExample : IExamplesProvider<IndicatorVersionDto>
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
