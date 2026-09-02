namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Reports;

using IAVH.BioTablero.CM.Application.DTOs.Reports;

using IAVH.BioTablero.CM.WebApi.Interfaces;

/// <summary>
/// Initiative statistics response example.
/// </summary>
public class InitiativeStatsResponseExample : IOpenApiExampleProvider<InitiativeStatsDto>
{
    /// <inheritdoc/>
    public InitiativeStatsDto GetExamples() =>
        new()
        {
            TotalIndicators = 1,
            TotalMunicipalities = 1,
        };
}
