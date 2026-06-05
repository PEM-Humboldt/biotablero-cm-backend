namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Reports;

using IAVH.BioTablero.CM.Application.DTOs.Reports;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Indicators statistics response example.
/// </summary>
public class IndicatorsStatsResponseExample : IExamplesProvider<IndicatorsStatsDto>
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
