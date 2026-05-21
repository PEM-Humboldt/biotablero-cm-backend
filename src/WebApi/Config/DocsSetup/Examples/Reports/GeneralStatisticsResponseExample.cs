namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Reports;

using IAVH.BioTablero.CM.Application.DTOs.Reports;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// General statistics response example.
/// </summary>
public class GeneralStatisticsResponseExample : IExamplesProvider<GeneralStatsDto>
{
    /// <inheritdoc/>
    public GeneralStatsDto GetExamples() =>
        new()
        {
            TotalActiveInitiatives = 12,
            TotalPeopleInvolved = 45,
            TotalAreaInHectares = 125075.50,
        };
}
