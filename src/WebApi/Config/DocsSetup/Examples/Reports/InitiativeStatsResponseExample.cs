namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Reports;

using IAVH.BioTablero.CM.Application.DTOs.Reports;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Initiative statistics response example.
/// </summary>
public class InitiativeStatsResponseExample : IExamplesProvider<InitiativeStatsDto>
{
    /// <inheritdoc/>
    public InitiativeStatsDto GetExamples() =>
        new()
        {
            TotalIndicators = 1,
            TotalMunicipalities = 1,
        };
}
