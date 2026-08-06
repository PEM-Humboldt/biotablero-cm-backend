namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Reports;

using IAVH.BioTablero.CM.Application.DTOs.Reports;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Report Data add request example.
/// </summary>
public class ReportDataAddRequestExample : IExamplesProvider<ReportDataDto>
{
    /// <inheritdoc/>
    public ReportDataDto GetExamples() => new()
    {
        Description = "Description example",
        Data = "Data example",
    };
}
