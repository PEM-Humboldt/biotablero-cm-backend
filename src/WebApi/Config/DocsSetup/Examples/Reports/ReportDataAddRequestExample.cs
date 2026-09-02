namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Reports;

using IAVH.BioTablero.CM.Application.DTOs.Reports;

using IAVH.BioTablero.CM.WebApi.Interfaces;

/// <summary>
/// Report Data add request example.
/// </summary>
public class ReportDataAddRequestExample : IOpenApiExampleProvider<ReportDataDto>
{
    /// <inheritdoc/>
    public ReportDataDto GetExamples() => new()
    {
        Description = "Description example",
        Data = "Data example",
    };
}
