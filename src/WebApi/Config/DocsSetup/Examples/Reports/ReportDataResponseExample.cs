namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Reports;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Reports;

using IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Attributes;

/// <summary>
/// Report Data response example.
/// </summary>
public class ReportDataResponseExample : IOpenApiExampleProvider<ReportDataDto>
{
    /// <inheritdoc/>
    public ReportDataDto GetExamples() => new()
    {
        Id = 0,
        UserName = "user-example@example.com",
        CreationDate = DateTimeOffset.UtcNow,
        Description = "Description example",
        Data = "Data example",
    };
}
