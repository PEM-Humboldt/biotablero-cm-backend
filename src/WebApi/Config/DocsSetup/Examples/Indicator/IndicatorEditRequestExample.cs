namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Indicator;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Indicator edit response example.
/// </summary>
public class IndicatorEditRequestExample : IExamplesProvider<IndicatorDto>
{
    /// <inheritdoc/>
    public IndicatorDto GetExamples() => new()
    {
        Name = "Indicator example (edited)",
    };
}
