namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Indicator;

using IAVH.BioTablero.CM.Application.DTOs.Indicators;

using IAVH.BioTablero.CM.WebApi.Interfaces;

/// <summary>
/// Indicator edit response example.
/// </summary>
public class IndicatorEditRequestExample : IOpenApiExampleProvider<IndicatorDto>
{
    /// <inheritdoc/>
    public IndicatorDto GetExamples() => new()
    {
        Name = "Indicator example (edited)",
    };
}
