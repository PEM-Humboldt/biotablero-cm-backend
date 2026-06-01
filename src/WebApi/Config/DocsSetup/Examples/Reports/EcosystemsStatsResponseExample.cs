namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Reports;

using IAVH.BioTablero.CM.Application.DTOs.Reports;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Ecosystems statistics response example.
/// </summary>
public class EcosystemsStatsResponseExample : IExamplesProvider<EcosystemsStatsDto>
{
    /// <inheritdoc/>
    public EcosystemsStatsDto GetExamples() =>
        new()
        {
            EcosystemsInvolved = [
                new()
                {
                    Id = 0,
                    Name = "Pastos Marinos",
                },
                new()
                {
                    Id = 1,
                    Name = "Humedales",
                },
            ],
        };
}
