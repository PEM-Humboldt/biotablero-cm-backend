namespace IAVH.BioTablero.CM.WebApi.Config.DocsSetup.Examples.Logging;

using System.Collections.Generic;

using IAVH.BioTablero.CM.Application.DTOs.Logging;

using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Log OData response example.
/// </summary>
public class LogOdataResponseExample : IExamplesProvider<Dictionary<string, object>>
{
    /// <summary>
    /// Get examples for entity.
    /// </summary>
    /// <returns>Entity examples.</returns>
    public Dictionary<string, object> GetExamples() => new()
    {
        ["@odata.count"] = 1,
        ["value"] = new List<LogDto>()
        {
            new(),
        },
    };
}
