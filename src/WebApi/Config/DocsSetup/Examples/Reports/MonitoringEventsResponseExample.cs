namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Reports;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Core.Domain.Models.Initiatives;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// General statistics response example.
/// </summary>
public class MonitoringEventsResponseExample : IExamplesProvider<List<MonitoringEventsData>>
{
    /// <inheritdoc/>
    public List<MonitoringEventsData> GetExamples() =>
    [
        new()
        {
            GroupNumber = 1,
            GroupName = "January",
            Value = 4,
        },

        new()
        {
            GroupNumber = 2,
            GroupName = "February",
            Value = 3,
        }
    ];
}
