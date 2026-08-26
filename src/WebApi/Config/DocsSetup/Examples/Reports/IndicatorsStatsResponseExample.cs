namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Reports;

using IAVH.BioTablero.CM.Application.DTOs.Reports;

using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Attributes;

/// <summary>
/// Indicators statistics response example.
/// </summary>
public class IndicatorsStatsResponseExample : IOpenApiExampleProvider<IndicatorsStatsDto>
{
    /// <inheritdoc/>
    public IndicatorsStatsDto GetExamples() =>
        new()
        {
            IndicatorsByScale =
            [
                new("Genes", 1),
                new("Especies", 1),
                new("Comunidades", 1),
            ],
        };
}
