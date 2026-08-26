namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Reports;

using IAVH.BioTablero.CM.Application.DTOs.Reports;

using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Attributes;

/// <summary>
/// General statistics response example.
/// </summary>
public class GeneralStatsResponseExample : IOpenApiExampleProvider<GeneralStatsDto>
{
    /// <inheritdoc/>
    public GeneralStatsDto GetExamples() =>
        new()
        {
            EnabledInitiatives = 12,
            PeopleInvolved = 45,
            Area = 125075.50,
        };
}
