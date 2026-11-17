namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Reports;

using System;

using IAVH.BioTablero.CM.Application.DTOs.Reports;

/// <summary>
/// Report Data OData response example.
/// </summary>
public class ReportDataOdataResponseExample : BaseOdataResponseExample<ReportDataDto>
{
    /// <inheritdoc/>
    protected override ReportDataDto CreateExampleDto() => new()
    {
        Id = 0,
        UserName = "user-example@example.com",
        CreationDate = DateTimeOffset.UtcNow,
        Description = "Description example",
        Data = "Data example",
    };
}
